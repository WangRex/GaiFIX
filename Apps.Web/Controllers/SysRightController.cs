using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Apps.BLL;
using Apps.Models;
using Apps.Common;
using Apps.Models.Sys;
using Apps.Web.Core;
using Apps.BLL.Sys;

namespace Apps.Web.Controllers
{
    public class SysRightController : BaseController
    {
        //
        // GET: /SysRight/

        public SysRightBLL sysRightBLL = new SysRightBLL();


        public SysRoleBLL sysRoleBLL = new SysRoleBLL();


        public SysModuleBLL sysModuleBLL = new SysModuleBLL();

        public SysModuleOperateBLL sysModuleOperateBLL = new SysModuleOperateBLL();
        //[SupportFilter]
        public ActionResult Index()
        {
            
            return View();
        }
        //获取角色列表
        //[SupportFilter(ActionName = "Index")]
        [HttpPost]
        public JsonResult GetRoleList(GridPager pager)
        {
            List<SysRole> list = sysRoleBLL._SysRoleRepository.FindPageList(ref pager).ToList();
            var json = new
            {
                total = pager.totalRows,
                rows = (from r in list
                        select new SysRole()
                        {

                            Id = r.Id,
                            Name = r.Name,
                            Description = r.Description,
                            CreateTime = r.CreateTime,
                           CreatePerson = r.CreatePerson

                        }).ToArray()

            };

            return Json(json);
        }
        //获取模组列表
        //[SupportFilter(ActionName = "Index")]
        [HttpPost]
        public JsonResult GetModelList(string id)
         {
             if (id == null)
                 id = "0";
             List<SysModule> list = sysModuleBLL.m_Rep.FindList(a => a.ParentId == id).ToList();
             var json = from r in list
                        select new SysModule()
                        {
                            Id = r.Id,
                            Name = r.Name,
                            EnglishName = r.EnglishName,
                            ParentId = r.ParentId,
                            Url = r.Url,
                            Iconic = r.Iconic,
                            Sort = r.Sort,
                            Remark = r.Remark,
                            Enable = r.Enable,
                           CreatePerson = r.CreatePerson,
                            CreateTime = r.CreateTime,
                            IsLast = r.IsLast,
                            state = (sysModuleBLL.m_Rep.FindList(a => a.ParentId == r.Id.ToString()).Count() > 0) ? "closed" : "open"
                        };


             return Json(json);
         }

        //根据角色与模块得出权限
        //[SupportFilter(ActionName = "Index")]
        [HttpPost]
        public JsonResult GetRightByRoleAndModule(GridPager pager, string roleId, string moduleId)
         {
             pager.rows = 100000;
             List<SysModuleOperate> right = sysModuleOperateBLL.m_Rep.FindList(a => a.ModuleId == moduleId).ToList();
             var json = new
             {
                 total = pager.totalRows,
                 rows = (from r in right
                         select new 
                         {
                            Ids= r.Id.ToString(),
                            Name= r.Name,
                            KeyCode =r.KeyCode,
                            IsValid = sysRightBLL._SysRightRepository.FindList(a => a.RoleId == roleId && a.ModuleId == r.ModuleId && a.MopId == r.Id.ToString()).ToList().Count == 0 ? false : true,
                            RightId = r.ModuleId
                         }).ToArray()

             };

             return Json(json);
         }
        //保存
        [HttpPost]
        //[SupportFilter(ActionName = "Save")]
        public Boolean UpdateRight(SysRight model)
        {
            List<SysRight> right = sysRightBLL._SysRightRepository.FindList(a => a.RoleId == model.RoleId && a.ModuleId == model.ModuleId && a.MopId == model.MopId).ToList();
            if (right.Count == 0 && model.IsValid == "true")
            {
                sysRightBLL._SysRightRepository.Create(model);
            }
            else if (right.Count != 0)
            {
                sysRightBLL._SysRightRepository.Delete(right);
            }
            return true;
        }


    }
}
