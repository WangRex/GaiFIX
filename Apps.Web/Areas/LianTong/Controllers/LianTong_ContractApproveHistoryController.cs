using Apps.BLL.LianTong;
using Apps.Models.LianTong;
using Apps.Web.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Apps.Web.Areas.LianTong.Controllers
{
    public class LianTong_ContractApproveHistoryController : BaseController
    {
        private LianTong_ProjectContractsApproveHisBLL appHis_BLL = new LianTong_ProjectContractsApproveHisBLL();
        public ActionResult Index(int ContractId)
        {
            ViewBag.ContractId = ContractId;
            return View();
        }

        /// <summary>
        /// 获取当前合同对应的审批记录
        /// </summary>
        /// <param name="ContractID">合同ID</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetList(int ContractID)
        {
            List<LianTong_ContractApproveHistoryModels> list;
            list = appHis_BLL.m_Rep.FindList(his => his.ContractID == ContractID).OrderByDescending(his => his.ApproveDate).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}