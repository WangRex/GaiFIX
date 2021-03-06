﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由T4模板自动生成
//	   生成时间 2012-12-11 22:05:42 by HD
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Apps.BLL.MIS;
using Apps.Common;
using Apps.Models;
using Apps.Models.MIS;

using System.Text;
using Apps.Web.Core;
using Apps.Locale;
using System;

namespace Apps.Web.Areas.MIS.Controllers
{
    public class CategoryController : BaseController
    {

        public MIS_Article_CategoryBLL m_BLL = new MIS_Article_CategoryBLL();
        ValidationErrors errors = new ValidationErrors();

        [SupportFilter]
        public ActionResult Index()
        {
            
            return View();
        }
        [HttpPost]
        public JsonResult GetList(string id)
        {
            if (id == null)
                id = "0";
            List<MIS_Article_Category> list = m_BLL.m_Rep.FindList(a => a.ParentId == id).ToList();
            var json = from r in list
                       select new MIS_Article_Category()
                       {
                           Id = r.Id,
                           Name = r.Name,
                           ParentId = r.ParentId,
                           Sort = r.Sort,
                           Enable = r.Enable,
                           CreateTime = r.CreateTime,
                           state = (m_BLL.m_Rep.FindList(a => a.ParentId == r.Id.ToString()).ToList().Count > 0) ? "closed" : "open"
                       };


            return Json(json);
        }

        /// <summary>
        /// 获取下拉列表的集合
        /// </summary>
        /// <param name="id">父ID</param>
        /// <returns></returns>
        public JsonResult GetListByParentId(string id)
        {
            var list = m_BLL.GetListByParentId(id);
            StringBuilder sb = new StringBuilder();
            foreach (var l in list)
            {
                sb.AppendFormat("<option value='{0}'>{1}</option>", l.Id, l.Name);
            }
            return Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        }

        #region 创建
        [SupportFilter]
        public ActionResult Create(string id)
        {
            
            MIS_Article_Category entity = new MIS_Article_Category()
            {
                ParentId = id,
                Enable = "true"
            };
            return View(entity);
        }

        [HttpPost]
        [SupportFilter]
        public JsonResult Create(MIS_Article_Category model)
        {
            model.CreateTime = ResultHelper.NowTime.ToString("yyyy-MM-dd");
            if (model != null && ModelState.IsValid)
            {

                if (m_BLL.m_Rep.Create(model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name, "成功", "创建", "MIS_Article_Category");
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name + "," + ErrorCol, "失败", "创建", "MIS_Article_Category");
                    return Json(JsonHandler.CreateMessage(0, Resource.InsertFail + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.InsertFail));
            }
        }
        #endregion

        #region 修改
        [SupportFilter]
        public ActionResult Edit(string id)
        {
            
            MIS_Article_Category entity = m_BLL.m_Rep.Find(Convert.ToInt32(id));
            return View(entity);
        }

        [HttpPost]
        [SupportFilter]
        public JsonResult Edit(MIS_Article_Category model)
        {
            if (model != null && ModelState.IsValid)
            {

                if (m_BLL.m_Rep.Update(model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name, "成功", "修改", "MIS_Article_Category");
                    return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name + "," + ErrorCol, "失败", "修改", "MIS_Article_Category");
                    return Json(JsonHandler.CreateMessage(0, Resource.EditFail + ":"+ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.EditFail));
            }
        }
        #endregion

        #region 详细

        public ActionResult Details(string id)
        {
            //获取父级
            List<MIS_Article_Category> list = m_BLL.m_Rep.FindList(a => a.ParentId == "0").ToList();

            foreach (var model in list)
            {
                model.clildren = m_BLL.m_Rep.FindList(a => a.ParentId == model.Id.ToString()).ToList();
                foreach (var m in model.clildren)
                {
                    m.clildren = m_BLL.m_Rep.FindList(a => a.ParentId == m.Id.ToString()).ToList();
                }
            }

            return View(list);
        }

        #endregion

        #region 删除
        [HttpPost]
        [SupportFilter]
        public JsonResult Delete(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                if (m_BLL.m_Rep.Delete(Convert.ToInt32(id))>0)
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + id, "成功", "删除", "MIS_Article_Category");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + id + "," + ErrorCol, "失败", "删除", "MIS_Article_Category");
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
