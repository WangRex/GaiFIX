using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Apps.Common;
using Apps.Models;
using Microsoft.Practices.Unity;
using Apps.BLL;
using Apps.Models.Sys;
using System;
using Apps.Web.Core;
using Apps.Locale;
using Apps.BLL.Sys;
using System.Data.SqlClient;

namespace Apps.Web.Controllers
{
    public class SysUserController : BaseController
    {
        //
        // GET: /SysUser/
        public SysUserBLL m_BLL = new SysUserBLL();
        public SysAreasBLL areasBLL = new SysAreasBLL();
        public SysRoleBLL roleBLL = new SysRoleBLL();
        public SysStructBLL structBLL = new SysStructBLL();
        public SysPositionBLL positionBLL = new SysPositionBLL();
        public SysRoleSysUserBLL roleSysUserBLL = new SysRoleSysUserBLL();



        ValidationErrors errors = new ValidationErrors();
        //[SupportFilter]
        public ActionResult Index()
        {
            return View();
        }
        //[SupportFilter(ActionName = "Index")]
        public JsonResult GetList(GridPager pager, string queryStr)
        {
            List<SysUser> list = m_BLL.GetList(ref pager, queryStr);
            var json = new
            {
                total = pager.totalRows,
                rows = list
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LookUp(string owner)
        {
            if (string.IsNullOrEmpty(owner))
            {
                ViewBag.owner = "1";
            }
            else
            {
                ViewBag.owner = owner;
            }
            return View();
        }


        //#region 设置用户角色
        //[SupportFilter(ActionName = "Allot")]
        public ActionResult GetRoleByUser(string userId)
        {
            ViewBag.UserId = userId;
            return View();
        }

        //[SupportFilter(ActionName = "Allot")]
        public JsonResult GetRoleListByUser(GridPager pager, string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return Json(0);
            var roleList = m_BLL.GetRoleListByUser(userId);
            var jsonData = new
            {
                total = pager.totalRows,
                rows = (
                    from r in roleList
                    select new
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Description = r.Description,
                        Flag = r.Flag == "0" ? "0" : "1",
                    }
                ).ToArray()
            };
            return Json(jsonData);
        }
        //[SupportFilter(ActionName = "Save")]
        public JsonResult UpdateUserRoleByUserId(string userId, string roleIds)
        {
            if (roleSysUserBLL.UpdateRolesByUserId(userId, roleIds))
            {
                LogHandler.WriteServiceLog(GetUserId(), "Ids:" + roleIds, "成功", "分配角色", "用户设置");
                return Json(JsonHandler.CreateMessage(1, Resource.SetSucceed), JsonRequestBehavior.AllowGet);
            }
            else
            {
                string ErrorCol = errors.Error;
                LogHandler.WriteServiceLog(GetUserId(), "Ids:" + roleIds, "失败", "分配角色", "用户设置");
                return Json(JsonHandler.CreateMessage(0, Resource.SetFail), JsonRequestBehavior.AllowGet);
            }
        }

        #region 创建
        //[SupportFilter]
        public ActionResult Create()
        {
            ViewBag.Struct = new SelectList(structBLL.m_Rep.FindList(a => a.ParentId == "0"), "Id", "Name");
            ViewBag.Areas = new SelectList(areasBLL._SysAreasRepository.FindList(a => a.ParentId == "0"), "KEY_Id", "Name");
            SysUser model = new SysUser()
            {
                Password = "123456",
                JoinDate = ResultHelper.NowTime.ToString("yyyy-MM-dd")
        };
            return View(model);
        }

        [HttpPost]
        //[SupportFilter]
        public JsonResult Create(SysUser model)
        {
            if (model != null)
            {
                model.CreateTime = ResultHelper.NowTime.ToString("yyyy-MM-dd");
                model.Password = ValueConvert.MD5(model.Password);
                model.CreatePerson = GetUserTrueName();
                if (!string.IsNullOrEmpty(model.DepId))
                {
                    model.DepName = structBLL.m_Rep.Find(Convert.ToInt32(model.DepId)).Name;
                }
                if (!string.IsNullOrEmpty(model.PosId))
                {
                    model.PosName = positionBLL.m_Rep.Find(Convert.ToInt32(model.PosId)).Name;
                }
                if (!string.IsNullOrEmpty(model.Province) && !"--未选择--".Equals(model.Province))
                {
                    model.ProvinceName = areasBLL._SysAreasRepository.Find(a => a.KEY_Id == model.Province).Name;
                }
                if (!string.IsNullOrEmpty(model.City) && !"--未选择--".Equals(model.City))
                {
                    model.CityName = areasBLL._SysAreasRepository.Find(a => a.KEY_Id == model.City).Name;
                }
                if (!string.IsNullOrEmpty(model.Village) && !"--未选择--".Equals(model.Village))
                {
                    model.VillageName = areasBLL._SysAreasRepository.Find(a => a.KEY_Id == model.Village).Name;
                }

                model.State = "true";
                if (m_BLL.m_Rep.Create(model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + model.Id + ",Name:" + model.UserName, "成功", "创建", "用户设置");
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + model.Id + ",Name:" + model.UserName + "," + ErrorCol,

"失败", "创建", "用户设置");
                    return Json(JsonHandler.CreateMessage(0, Resource.InsertFail + ErrorCol),

JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.InsertFail), JsonRequestBehavior.AllowGet);
            }
        }
        //判断是否用户重复
        [HttpPost]
        public JsonResult JudgeUserName(string userName)
        {
            return Json("用户名已经存在！", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 修改
        //[SupportFilter]
        public ActionResult Edit(string id)
        {
            ViewBag.Areas = new SelectList(areasBLL._SysAreasRepository.FindList(a => a.ParentId == "0"), "KEY_Id", "Name");
            SysUser entity = m_BLL.m_Rep.Find(Convert.ToInt32(id));
            return View(entity);
        }

        [HttpPost]
        //[SupportFilter]
        public JsonResult Edit(SysUser info)
        {
            if (info != null)
            {
                if (!string.IsNullOrEmpty(info.DepId))
                {
                    info.DepName = structBLL.m_Rep.Find(Convert.ToInt32(info.DepId)).Name;
                }
                if (!string.IsNullOrEmpty(info.PosId))
                {
                    info.PosName = positionBLL.m_Rep.Find(Convert.ToInt32(info.PosId)).Name;
                }
                if (!string.IsNullOrEmpty(info.Province) && !"--未选择--".Equals(info.Province))
                {
                    info.ProvinceName = areasBLL._SysAreasRepository.Find(a => a.KEY_Id ==info.Province).Name;
                }
                if (!string.IsNullOrEmpty(info.City) && !"--未选择--".Equals(info.City))
                {
                    info.CityName = areasBLL._SysAreasRepository.Find(a => a.KEY_Id == info.City).Name;
                }
                if (!string.IsNullOrEmpty(info.Village) && !"--未选择--".Equals(info.Village))
                {
                    info.VillageName = areasBLL._SysAreasRepository.Find(a => a.KEY_Id == info.Village).Name;
                }

                if (m_BLL.m_Rep.Update(info))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + info.Id + ",Name:" + info.UserName, "成功", "修改", "用户设置");
                    return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + info.Id + ",Name:" + info.UserName + "," + ErrorCol, "失败", "修改", "用户设置");
                    return Json(JsonHandler.CreateMessage(0, Resource.EditFail + ":" + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.EditFail), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        //[SupportFilter(ActionName = "Edit")]
        public JsonResult ReSet(string Id, string Pwd)
        {
            SysUser editModel = m_BLL.m_Rep.Find(Convert.ToInt32(Id));
            editModel.Password = ValueConvert.MD5(Pwd);
            if (m_BLL.m_Rep.Update(editModel))
            {
                LogHandler.WriteServiceLog(GetUserId(), "Id:" + Id + ",密码:********", "成功", "初始化密码", "用户设置");
                return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed), JsonRequestBehavior.AllowGet);
            }
            else
            {
                string ErrorCol = errors.Error;
                LogHandler.WriteServiceLog(GetUserId(), "Id:" + Id + ",,密码:********" + ErrorCol, "失败", "初始化密码", "用户设置");
                return Json(JsonHandler.CreateMessage(0, Resource.EditFail + ":" + ErrorCol), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 详细
        //[SupportFilter]
        public ActionResult Details(string id)
        {

            SysUser entity = m_BLL.m_Rep.Find(Convert.ToInt32(id));
            return View(entity);
        }

        #endregion

        #region 删除
        [HttpPost]
        //[SupportFilter]
        public JsonResult Delete(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                //保护管理员不能被删除
                //if (id == "admin")
                //{
                //    LogHandler.WriteServiceLog(GetUserId(), "尝试删除管理员", "失败", "删除", "用户设置");
                //    return Json(JsonHandler.CreateMessage(0, "管理员不能被删除！"), JsonRequestBehavior.AllowGet);
                //}
                if (m_BLL.m_Rep.Delete(Convert.ToInt32(id)) > 0)
                {
                    var delete = roleSysUserBLL.m_Rep.FindList(a => a.SysUserId == id);
                    roleSysUserBLL.m_Rep.Delete(delete);
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + id, "成功", "删除", "用户设置");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + id + "," + ErrorCol, "失败", "删除", "用户设置");
                    return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion





    }
}
