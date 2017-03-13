using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Apps.Common;

using Apps.Models.Sys;
using Microsoft.Practices.Unity;
using Apps.Models.Flow;
using Apps.BLL.Flow;
using Apps.Web.Core;
using Apps.Locale;
using System;

namespace Apps.Web.Areas.Flow.Controllers
{
    public class FlowTypeController : BaseController
    {
        
        public Flow_TypeBLL m_BLL = new Flow_TypeBLL();
        ValidationErrors errors = new ValidationErrors();

        //[SupportFilter]
        public ActionResult Index()
        {
            
            return View();
        }
        [HttpPost]
        public JsonResult GetList(GridPager pager, string queryStr)
        {
            List<Flow_Type> list = m_BLL.m_Rep.FindPageList(ref pager, queryStr).ToList();
            var json = new
            {
                total = pager.totalRows,
                rows = (from r in list
                        select new Flow_Type()
                        {

                            Id = r.Id,
                            Name = r.Name,
                            Remark = r.Remark,
                            CreateTime = r.CreateTime,
                            Sort = r.Sort

                        }).ToArray()

            };

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
        public JsonResult Create(Flow_Type model)
        {
            model.CreateTime = ResultHelper.NowTime.ToString("yyyy-MM-dd");
            if (model != null)
            {

                if (m_BLL.m_Rep.Create( model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name, "成功", "创建", "Flow_Type");
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name + "," + ErrorCol, "失败", "创建", "Flow_Type");
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
            
            Flow_Type entity = m_BLL.m_Rep.Find(Convert.ToInt32(id));
            return View(entity);
        }

        [HttpPost]
        //[SupportFilter]
        public JsonResult Edit(Flow_Type model)
        {
            if (model != null)
            {

                if (m_BLL.m_Rep.Update(model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name, "成功", "修改", "Flow_Type");
                    return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name + "," + ErrorCol, "失败", "修改", "Flow_Type");
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
        //[SupportFilter]
        public ActionResult Details(string id)
        {
            
            Flow_Type entity = m_BLL.m_Rep.Find(Convert.ToInt32(id));
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
                if (m_BLL.m_Rep.Delete(Convert.ToInt32(id))> 0)
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + id, "成功", "删除", "Flow_Type");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + id + "," + ErrorCol, "失败", "删除", "Flow_Type");
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
