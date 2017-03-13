using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Apps.Common;
using Apps.Models;
using Microsoft.Practices.Unity;
using Apps.BLL;
using Apps.Models.Sys;
using Apps.Web.Core;
using Apps.Locale;
using Apps.BLL.Sys;

namespace Apps.Web.Controllers
{
    public class SysLogController : BaseController
    {
        //
        // GET: /SysLog/
        
        public SysLogBLL logBLL { get; set; }
        ValidationErrors errors = new ValidationErrors();

        ////[SupportFilter]
        public ActionResult Index()
        {
            
            return View();

        }

        //个人记录
        public ActionResult MyLog()
        {
            return View();
        }
        public JsonResult GetListByUser(GridPager pager, string queryStr)
        {

            List<SysLog> list = new List<SysLog>();
            var json = new
            {
                total = pager.totalRows,
                rows = (from r in list
                        select new SysLog()
                        {

                            Id = r.Id,
                            Operator = r.Operator,
                            Message = r.Message,
                            Result = r.Result,
                            Type = r.Type,
                            Module = r.Module,
                            CreateTime = r.CreateTime

                        }).ToArray()

            };

            return Json(json);
        }



        //[SupportFilter(ActionName = "Index")]
        public JsonResult GetList(GridPager pager, string queryStr)
        {
            List<SysLog> list ;
            GridRows<SysLog> grs = new GridRows<SysLog>();
            //grs.rows = list;
            grs.total = pager.totalRows;
            return Json(grs);
          
        }


        //#region 详细
        //////[SupportFilter]
        //public ActionResult Details(string id)
        //{
            
        //    SysLog entity = logBLL.GetById(id);

        //    return View(entity);
        //}

        //#endregion

        //#region 删除
        //[HttpPost]
        //////[SupportFilter]
        //public JsonResult Delete(string ids)
        //{
        //    if (!string.IsNullOrWhiteSpace(ids))
        //    {
        //        string[] deleteIds = ids.Split(',');
        //        if (logBLL.Delete(ref errors, deleteIds))
        //        {
        //            return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed), JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            string ErrorCol = errors.Error;
        //            return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail + ErrorCol), JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    else
        //    {
        //        return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail), JsonRequestBehavior.AllowGet);
        //    }


        //}
        //#endregion
    }
}
