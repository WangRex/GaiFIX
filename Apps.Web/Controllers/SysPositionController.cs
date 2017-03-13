using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Apps.Common;
using Apps.BLL;
using Apps.Models.Sys;
using Microsoft.Practices.Unity;
using Apps.Web;
using Apps.Web.Core;
using Apps.Locale;
using Apps.BLL.Sys;
using System;

namespace Apps.Web.Controllers
{
    public class SysPositionController : BaseController
    {

        public SysPositionBLL m_BLL = new SysPositionBLL();
        ValidationErrors errors = new ValidationErrors();

        //////[SupportFilter]
        public ActionResult Index()
        {
            
            return View();
        }
        [HttpPost]
        public JsonResult GetList(GridPager pager, string queryStr)
        {
            if (string.IsNullOrEmpty(queryStr))
            {
                queryStr = string.Empty;
            }
            List<SysPosition> list = m_BLL.m_Rep.FindPageList(ref pager, a => a.Id.ToString().Contains(queryStr)
                                || a.Name.Contains(queryStr)
                                || a.Remark.Contains(queryStr)
                                || a.DepId.Contains(queryStr)).ToList();
            var json = new
            {
                total = pager.totalRows,
                rows = list
            };

            return Json(json);
        }
        [HttpPost]
        public JsonResult GetPosListByComTree(string depId)
        {
            List<SysPosition> list = m_BLL.m_Rep.FindPageList(ref setNoPagerAscBySort, a => a.DepId == depId).ToList();
            var json = from r in list
                       select new
                       {
                           id = r.Id.ToString(),
                           text = r.Name,
                           state = "open"
                       };


            return Json(json);
        }

        public List<SysPosition> GetPosListByDepId(ref GridPager pager, string depId)
        {

            IQueryable<SysPosition> queryData = null;
            if (!string.IsNullOrWhiteSpace(depId))
            {
                if (depId == "root")
                    queryData = m_BLL.m_Rep.FindList();
                else
                    queryData = m_BLL.m_Rep.FindList(a => a.DepId == depId);
            }
            else
            {
                queryData = m_BLL.m_Rep.FindList();
            }
            pager.totalRows = queryData.Count();
            //queryData = LinqHelper.SortingAndPaging(queryData, pager.sort, pager.order, pager.page, pager.rows);
            return queryData.ToList();
        }

        #region 创建
        //////[SupportFilter]
        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        //////[SupportFilter]
        public JsonResult Create(SysPosition model)
        {
            model.CreateTime = ResultHelper.NowTime.ToString("yyyy-MM-dd");
            if (model != null)
            {
                if (m_BLL.m_Rep.Create(model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name, "成功", "创建", "SysPosition");
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name + "," + ErrorCol, "失败", "创建", "SysPosition");
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
        //////[SupportFilter]
        public ActionResult Edit(string id)
        {

            SysPosition entity = m_BLL.m_Rep.Find(Convert.ToInt32(id));
            return View(entity);
        }

        [HttpPost]
        //////[SupportFilter]
        public JsonResult Edit(SysPosition model)
        {
            if (model != null)
            {
                if (m_BLL.m_Rep.Update(model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name, "成功", "修改", "SysPosition");
                    return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name + "," + ErrorCol, "失败", "修改", "SysPosition");
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

            SysPosition entity = m_BLL.m_Rep.Find(Convert.ToInt32(id));
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
                if (m_BLL.m_Rep.Delete(Convert.ToInt32(id))>0)
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + id, "成功", "删除", "SysPosition");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + id + "," + ErrorCol, "失败", "删除", "SysPosition");
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
