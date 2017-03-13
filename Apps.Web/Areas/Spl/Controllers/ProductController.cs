using System.Collections.Generic;
using Apps.Web.Core;
using Apps.Locale;
using System.Web.Mvc;
using Apps.Common;
using Apps.Models.Spl;
using Microsoft.Practices.Unity;
using Apps.Models.Common;
using Apps.BLL.Spl;
using System;

namespace Apps.Web.Areas.Spl.Controllers
{
    public class ProductController : BaseController
    {
        public Spl_ProductBLL m_BLL = new Spl_ProductBLL();
        ValidationErrors errors = new ValidationErrors();

        //[SupportFilter]
        public ActionResult Index()
        {
            
            return View();
        }
        [HttpPost]
        //[SupportFilter(ActionName="Index")]
        public JsonResult GetList(GridPager pager, string queryStr)
        {
            List<Spl_Product> list = m_BLL.GetList(ref pager, queryStr);
            GridRows<Spl_Product> grs = new GridRows<Spl_Product>();
            grs.rows = list;
            grs.total = pager.totalRows;

            return Json(grs);
        }

        [HttpPost]
        //[SupportFilter(ActionName = "Index")]
        public JsonResult GetOptionByBarChart(GridPager pager, string queryStr)
        {
            List<Spl_Product> list = m_BLL.GetList(ref pager, queryStr);
            List<decimal?> costPrice = new List<decimal?>();
            list.ForEach(a => costPrice.Add(Convert.ToDecimal(a.CostPrice)));
            List<decimal?> price = new List<decimal?>();
            list.ForEach(a => price.Add(Convert.ToDecimal(a.Price)));
            List<string> names= new List<string>();
            list.ForEach(a=> names.Add(a. Name));
            List<ChartSeries> seriesList = new List<ChartSeries>();
            ChartSeries series1 = new ChartSeries() {
                name = "成本价",
                type = "bar",
                data = costPrice
            };
            ChartSeries series2 = new ChartSeries()
            {
                name = "零售价",
                type = "bar",
                data = price
            };
            seriesList.Add(series1);
            seriesList.Add(series2);
            var option= new
            {
                title= new{text= "成本价零售价对照表" }, 
                tooltip= new{},
                legend = new { data = "成本价零售价对照表" },
                xAxis= new{ data= names},
                yAxis= new{},
                series = seriesList
            };
            return Json(option);
        }

        #region 创建
        [SupportFilter]
        public ActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        //[SupportFilter]
        public JsonResult Create(Spl_Product model)
        {
            model.CreateTime = ResultHelper.NowTime.ToString("yyyy-MM-dd HH:mm:ss");
            if (model != null && ModelState.IsValid)
            {

                if (m_BLL.m_Rep.Create(model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name, "成功", "创建", "Spl_Product");
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name + "," + ErrorCol, "失败", "创建", "Spl_Product");
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
            
            Spl_Product entity = m_BLL.m_Rep.Find (Convert.ToInt32 (id));
            return View(entity);
        }

        [HttpPost]
        [SupportFilter]
        public JsonResult Edit(Spl_Product model)
        {
            if (model != null && ModelState.IsValid)
            {

                if (m_BLL.m_Rep .Update (model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name, "成功", "修改", "Spl_Product");
                    return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name + "," + ErrorCol, "失败", "修改", "Spl_Product");
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
        [SupportFilter]
        public ActionResult Details(string id)
        {
            
            Spl_Product entity = m_BLL.m_Rep.Find(Convert.ToInt32(id));
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
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + id, "成功", "删除", "Spl_Product");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + id + "," + ErrorCol, "失败", "删除", "Spl_Product");
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
