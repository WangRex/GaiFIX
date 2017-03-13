using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Apps.Common;
using Apps.Models;
using Microsoft.Practices.Unity;
using Apps.BLL;

using Apps.Models.Sys;
using Apps.Web.Core;
using Apps.Web;
using Apps.Locale;
using Apps.BLL.Sys;
using System;

namespace Apps.Web.Controllers
{
    public class SysRoleController : BaseController
    {
        //
        // GET: /SysRole/

        public SysRoleBLL m_BLL = new SysRoleBLL();
        public SysStructBLL structBLL = new SysStructBLL();
        ValidationErrors errors = new ValidationErrors();
        public SysUserBLL sysUserBLL = new SysUserBLL();
        public SysRoleSysUserBLL roleSysUserBLL = new SysRoleSysUserBLL();

        //////[SupportFilter]
        public ActionResult Index()
        {
            
            return View();
        }

        #region 设置角色用户
        ////[SupportFilter(ActionName = "Allot")]
        public ActionResult GetUserByRole(string roleId)
        {
            ViewBag.RoleId = roleId;
            
            CommonHelper commonHelper = new CommonHelper();
            ViewBag.StructTree = structBLL.GetStructTree(true);
            return View();
        }
        #endregion

        ////[SupportFilter(ActionName = "Allot")]
        public JsonResult GetUserListByRole(GridPager pager, string roleId, string depId, string queryStr)
        {
            if (string.IsNullOrWhiteSpace(roleId))
                return Json(0);
            var userList = m_BLL.GetRoleListByUser(roleId, depId);
            var jsonData = new
            {
                total = pager.totalRows,
                rows = (
                    from r in userList
                    select new
                    {
                        Id = r.Id,
                        UserName = r.UserName,
                        TrueName = r.TrueName,
                        Flag = r.Flag == "0" ? "0" : "1",
                    }
                ).ToArray()
            };
            return Json(jsonData);
        }

        #region 一览
        ////[SupportFilter(ActionName = "Index")]
        public JsonResult GetList(GridPager pager, string queryStr)
        {
            if (string.IsNullOrEmpty(queryStr))
            {
                queryStr = string.Empty;
            }
            List<SysRole> list = m_BLL._SysRoleRepository.FindPageList(ref pager, a => a.Name.Contains(queryStr) || a.Description.Contains(queryStr)).ToList();
            var json = new
            {
                total = pager.totalRows,
                rows = list

            };

            return Json(json);
        }
        #endregion

        public JsonResult UpdateUserRoleByRoleId(string RoleId, string userIds)
        {
            if(roleSysUserBLL.UpdateUsersByRoleId(RoleId, userIds))
            {
                return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.CheckFail));
            }
        }


        #region 创建
        //////[SupportFilter]
        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        //////[SupportFilter]
        public JsonResult Create(SysRole model)
        {
            model.CreateTime = ResultHelper.NowTime.ToString("yyyy-MM-dd HH:mm:ss");
            model.CreatePerson = GetUserId();
            if (m_BLL._SysRoleRepository.Create(model))
            {
                LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name, "成功", "创建", "SysRole");
                return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
            }
            else
            {
                string ErrorCol = errors.Error;
                LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name + "," + ErrorCol, "失败", "创建", "SysRole");
                return Json(JsonHandler.CreateMessage(0, Resource.InsertFail + ErrorCol));
            }
        }
        #endregion

        #region 修改
        //////[SupportFilter]
        public ActionResult Edit(string id)
        {

            SysRole entity = m_BLL._SysRoleRepository.Find(Convert.ToInt32(id));
            return View(entity);
        }

        [HttpPost]
        //////[SupportFilter]
        public JsonResult Edit(SysRole model)
        {
            if (model != null)
            {

                if (m_BLL._SysRoleRepository.Update(model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name, "成功", "修改", "SysRole");
                    return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name + "," + ErrorCol, "失败", "修改", "SysRole");
                    return Json(JsonHandler.CreateMessage(0, Resource.EditFail + ":" + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.EditFail));
            }
        }
        #endregion

        #region 详细
        //////[SupportFilter]
        public ActionResult Details(string id)
        {

            SysRole entity = m_BLL._SysRoleRepository.Find(Convert.ToInt32(id));
            return View(entity);
        }

        #endregion

        #region 删除
        [HttpPost]
        //////[SupportFilter]
        public JsonResult Delete(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                //if (id == "administrator")
                //{
                //    LogHandler.WriteServiceLog(GetUserId(), "尝试删除管理员组", "失败", "删除", "用户设置");
                //    return Json(JsonHandler.CreateMessage(0, "超级管理员组不能被删除！"), JsonRequestBehavior.AllowGet);
                //}
                if (m_BLL._SysRoleRepository.Delete(Convert.ToInt32(id))>0)
                {
                    var delete = roleSysUserBLL.m_Rep.FindList(a => a.SysRoleId == id);
                    roleSysUserBLL.m_Rep.Delete(delete);
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + id, "成功", "删除", "SysRole");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + id + "," + ErrorCol, "失败", "删除", "SysRole");
                    return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail));
            }


        }
        #endregion
    }
}