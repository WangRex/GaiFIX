using System.Collections.Generic;
using System.Web.Mvc;
using Apps.Common;
using Apps.BLL;
using Apps.Models.Sys;
using Microsoft.Practices.Unity;
using Apps.Web.Core;
using Apps.Locale;
using Apps.BLL.Sys;
//using Microsoft.Reporting.WebForms;

namespace Apps.Web.Controllers
{
    public class SysSampleController : BaseController
    {

        /// <summary>
        /// 业务层注入
        /// </summary>

        public SysSampleBLL m_BLL = new SysSampleBLL();
        ValidationErrors errors = new ValidationErrors();
        ////[SupportFilter]
        public ActionResult Index()
        {
            
            return View();
        }
        [HttpPost]
        //[SupportFilter(ActionName="Index")]
        public JsonResult GetList(GridPager pager, string queryStr)
        {
            List<SysSample> list;
            GridRows<SysSample> grs = new GridRows<SysSample>();
            //grs.rows = list;
            grs.total = pager.totalRows;

            return Json(grs);
        }
        #region 导出到PDF EXCEL WORD
        //public ActionResult Reporting(string type = "PDF", string queryStr = "", int rows = 0, int page = 1)
        //{
        //    //选择了导出全部
        //    if (rows == 0 && page == 0)
        //    {
        //        rows = 9999999;
        //        page = 1;
        //    }
        //    GridPager pager = new GridPager()
        //    {
        //        rows = rows,
        //        page = page,
        //        sort="Id",
        //        order="desc"
        //    };
        //    List<SysSample> ds = m_BLL.Find(ref pager, queryStr);
        //    LocalReport localReport = new LocalReport();
    
        //    localReport.ReportPath = Server.MapPath("~/Reports/SysSampleReport.rdlc");
            
        //    ReportDataSource reportDataSource = new ReportDataSource("DataSet1", ds);
        //    localReport.DataSources.Add(reportDataSource);
        //    string reportType = type;
        //    string mimeType;
        //    string encoding;
        //    string fileNameExtension;

        //    string deviceInfo =
        //        "<DeviceInfo>" +
        //        "<OutPutFormat>" + type + "</OutPutFormat>" +
        //        "<PageWidth>11in</PageWidth>" +
        //        "<PageHeight>11in</PageHeight>" +
        //        "<MarginTop>0.5in</MarginTop>" +
        //        "<MarginLeft>1in</MarginLeft>" +
        //        "<MarginRight>1in</MarginRight>" +
        //        "<MarginBottom>0.5in</MarginBottom>" +
        //        "</DeviceInfo>";
        //    Warning[] warnings;
        //    string[] streams;
        //    byte[] renderedBytes;

        //    renderedBytes = localReport.Render(
        //        reportType,
        //        deviceInfo,
        //        out mimeType,
        //        out encoding,
        //        out fileNameExtension,
        //        out streams,
        //        out warnings
        //        );
        //    return File(renderedBytes, mimeType);
        //}
        #endregion
    }
}
