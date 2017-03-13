using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Apps.Common;
using Apps.BLL;
using Apps.Models.Sys;
using Microsoft.Practices.Unity;
using Apps.BLL.MIS;
using Apps.Models.MIS;
using Apps.Web.Core;
using Apps.Locale;
using System;

namespace Apps.Web.Areas.MIS.Controllers
{
    public class WebIM_CommonTalkController : BaseController
    {

        public MIS_WebIM_CommonTalkBLL m_BLL = new MIS_WebIM_CommonTalkBLL();
        ValidationErrors errors = new ValidationErrors();

        [SupportFilter]
        public ActionResult Index()
        {
            
            return View();
        }
        [HttpPost]
        public JsonResult GetList(GridPager pager, string queryStr)
        {
            List<MIS_WebIM_CommonTalk> list = m_BLL.m_Rep.FindPageList(ref pager, queryStr).ToList();
            var json = new
            {
                total = pager.totalRows,
                rows = (from r in list
                        select new MIS_WebIM_CommonTalk()
                        {

                            Id = r.Id,
                            Talk = r.Talk,
                            State = r.State,
                            CreateTime = r.CreateTime

                        }).ToArray()

            };

            return Json(json);
        }

        #region 创建
        [SupportFilter]
        public ActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        [SupportFilter]
        public JsonResult Create(MIS_WebIM_CommonTalk model)
        {
            
            model.CreateTime = ResultHelper.NowTime.ToString("yyyy-MM-dd");
            if (model != null && ModelState.IsValid)
            {

                if (m_BLL.m_Rep.Create(model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Talk" + model.Talk, "成功", "创建", "MIS_WebIM_CommonTalk");
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Talk" + model.Talk + "," + ErrorCol, "失败", "创建", "MIS_WebIM_CommonTalk");
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
            
            MIS_WebIM_CommonTalk entity = m_BLL.m_Rep.Find(Convert.ToInt32(id));
            return View(entity);
        }

        [HttpPost]
        [SupportFilter]
        public JsonResult Edit(MIS_WebIM_CommonTalk model)
        {
            if (model != null && ModelState.IsValid)
            {

                if (m_BLL.m_Rep.Update(model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Talk" + model.Talk, "成功", "修改", "MIS_WebIM_CommonTalk");
                    return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Talk" + model.Talk + "," + ErrorCol, "失败", "修改", "MIS_WebIM_CommonTalk");
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
        [SupportFilter]
        public ActionResult Details(string id)
        {
            
            MIS_WebIM_CommonTalk entity = m_BLL.m_Rep.Find(Convert.ToInt32(id));
            return View(entity);
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
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + id, "成功", "删除", "MIS_WebIM_CommonTalk");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + id + "," + ErrorCol, "失败", "删除", "MIS_WebIM_CommonTalk");
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
