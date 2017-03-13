using System.Collections.Generic;
using System.Linq;
using Apps.Web.Core;
using Apps.Locale;
using System.Web.Mvc;
using Apps.Common;
using Apps.Models.Spl;
using Microsoft.Practices.Unity;
using Apps.BLL.Spl;
using System;

namespace Apps.Web.Areas.Spl.Controllers
{
    public class ProductCategoryController : BaseController
    {
        public Spl_ProductCategoryBLL m_BLL = new Spl_ProductCategoryBLL();
        ValidationErrors errors = new ValidationErrors();

        //[SupportFilter]
        public ActionResult Index()
        {
            
            return View();
        }
        [HttpPost]
        public JsonResult GetList(GridPager pager, string queryStr)
        {
            List<Spl_ProductCategory> list = m_BLL.m_Rep.FindPageList(ref pager, queryStr).ToList();
            GridRows<Spl_ProductCategory> grs = new GridRows<Spl_ProductCategory>();
            grs.rows = list;
            grs.total = pager.totalRows;

            return Json(grs);
        }

        [HttpPost]
        public JsonResult GetComboxData()
        {
            List<Spl_ProductCategory> list = m_BLL.m_Rep.FindPageList(ref setNoPagerDescByCreateTime, "").ToList();
            var json =(from r in list
                        select new Spl_ProductCategory()
                        {
                            Id = r.Id,
                            Name = r.Name
                        }).ToArray();

            return Json(json);
        }

        #region 创建
        //[SupportFilter]
        public ActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        //[SupportFilter]
        public JsonResult Create(Spl_ProductCategory model)
        {
            model.CreateTime = ResultHelper.NowTime.ToString("yyyy-MM-dd HH:mm:ss");
            if (model != null && ModelState.IsValid)
            {

                if (m_BLL.m_Rep.Create(model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name, "成功", "创建", "Spl_ProductCategory");
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name + "," + ErrorCol, "失败", "创建", "Spl_ProductCategory");
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
        //[SupportFilter]
        public ActionResult Edit(string id)
        {
            
            Spl_ProductCategory entity = m_BLL.m_Rep.Find(Convert.ToInt32(id));
            return View(entity);
        }

        [HttpPost]
        //[SupportFilter]
        public JsonResult Edit(Spl_ProductCategory model)
        {
            if (model != null && ModelState.IsValid)
            {

                if (m_BLL.m_Rep.Update(model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name, "成功", "修改", "Spl_ProductCategory");
                    return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name + "," + ErrorCol, "失败", "修改", "Spl_ProductCategory");
                    return Json(JsonHandler.CreateMessage(0, Resource.EditFail + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.EditFail));
            }
        }
        #endregion

        #region 详细
        //[SupportFilter]
        public ActionResult Details(string id)
        {
            
            Spl_ProductCategory entity = m_BLL.m_Rep.Find(Convert.ToInt32(id));
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
                if (m_BLL.m_Rep.Delete(Convert.ToInt32(id))>0)
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + id, "成功", "删除", "Spl_ProductCategory");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + id + "," + ErrorCol, "失败", "删除", "Spl_ProductCategory");
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
