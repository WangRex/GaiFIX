using Apps.BLL.Fix;
using Apps.BLL.Sys;
using Apps.Common;
using Apps.Models.FIX;
using Apps.Web.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Apps.Web.Areas.Fix.Controllers
{
    /// <summary>
    /// 评价管理
    /// </summary>
    public class Fix_AssessmentController : BaseController
    {
        private Fix_AssessmentBLL m_BLL = new Fix_AssessmentBLL();
        private SysStructBLL StructBLL = new SysStructBLL();
        ValidationErrors errors = new ValidationErrors();

        /// <summary>
        /// 首页跳转
        /// </summary>
        /// <returns></returns>
        [SupportFilter]
        public ActionResult Index()
        {
            return View();
        }
    }
}