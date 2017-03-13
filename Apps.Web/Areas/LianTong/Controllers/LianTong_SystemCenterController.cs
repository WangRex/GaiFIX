using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Apps.Common;
using System;
using Apps.Web.Core;
using Apps.Locale;
using Apps.BLL.Sys;
using Apps.BLL.LianTong;
using Apps.Models.LianTong;
using Newtonsoft.Json;
using Apps.Common.ExcelHelper;
using System.Data;

namespace Apps.Web.Areas.LianTong.Controllers
{
    public class LianTong_SystemCenterController : BaseController
    {
        private LianTong_SystemCenterBLL m_BLL = new LianTong_SystemCenterBLL();
        ValidationErrors errors = new ValidationErrors();

        #region 首页
        [SupportFilter]
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region 获取列表
        //[SupportFilter(ActionName = "Index")]
        public JsonResult GetList(GridPager pager, string queryStr)
        {
            if (string.IsNullOrEmpty(queryStr)) queryStr = "";
            List<LianTong_SystemCenterModel> list;
            list = m_BLL.m_Rep.FindPageList(ref pager, a => a.leaderName.Contains(queryStr) ||
            a.projectName.Contains(queryStr) ||
             a.projectAttribution.Contains(queryStr) ||
              a.AccountManager.Contains(queryStr) ||
               a.projectArea.Contains(queryStr) ||
                a.projectType.Contains(queryStr) ||
                 a.adress.Contains(queryStr) ||
                  a.contactWay.Contains(queryStr)
            ).ToList();

            var json = new
            {
                total = pager.totalRows,
                rows = (from r in list
                        select new
                        {
                            Id = r.Id,
                            leaderName = r.leaderName,
                            projectName = r.projectName,
                            projectAttribution = r.projectAttribution,
                            projectArea = r.projectArea,
                            projectType = r.projectType,
                            AccountManager = r.AccountManager,
                            AccountManagerTel = r.AccountManagerTel,
                            SystemSupport = r.SystemSupport,
                            equipmentDealerContacts = r.equipmentDealerContacts,
                            equipmentDealerTel = r.equipmentDealerTel,
                            adress = r.adress,
                            contactPeople = r.contactPeople,
                            contactWay = r.contactWay,
                            equipmentType = r.equipmentType,
                            equipmentDealer = r.equipmentDealer,
                            constructionDepartment = r.constructionDepartment,
                            contractCost = r.contractCost,
                            equipmentCost = r.equipmentCost,
                            projectCost = r.projectCost,
                            Profit = m_BLL.getProfit(r),
                            ProfitRate = m_BLL.getProfitRate(r),
                            maintenanceDepartment = r.maintenanceDepartment,
                            maintenancePeriod = r.maintenancePeriod,
                            contractStartDate = r.contractStartDate,
                            contractEndDate = r.contractEndDate,
                            Comments = r.Comments,
                            UpLoad = r.UpLoad

                        }).ToArray()
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 保存数据
        [HttpPost]
        public ActionResult CreatByGrid()
        {
            string result = Request.Form[0];

            //后台拿到字符串时直接反序列化。根据需要自己处理
            //var datagrid = JsonConvert.DeserializeObject<List<datagrid>>(result);
            List<LianTong_SystemCenterModel> datagridList = new List<LianTong_SystemCenterModel>();
            try
            { datagridList = JsonConvert.DeserializeObject<List<LianTong_SystemCenterModel>>(result); }
            catch (Exception)
            {
                string ErrorCol = "输入数据类型错误，请点撤销后重新输入";
                LogHandler.WriteServiceLog(GetUserId(), ErrorCol, "失败", "数据更新", "工程设置");
                return Json(JsonHandler.CreateMessage(0, Resource.SetFail), JsonRequestBehavior.AllowGet);
            }
            foreach (LianTong_SystemCenterModel info in datagridList)
            {
                if (info.Id > 0)
                {
                    if (m_BLL.m_Rep.Update(info))
                    {
                        LogHandler.WriteServiceLog(GetUserId(), "Id:" + info.Id + ",projectName:" + info.projectName, "成功", "修改", "工程设置");
                    }
                    else
                    {
                        string ErrorCol = errors.Error;
                        LogHandler.WriteServiceLog(GetUserId(), "Id:" + info.Id + ",projectName:" + info.projectName + "," + ErrorCol, "失败", "修改", "工程设置");
                        return Json(JsonHandler.CreateMessage(0, Resource.EditFail + ":" + ErrorCol));
                    }
                }
                else
                {
                    if (m_BLL.m_Rep.Create(info))
                    {
                        LogHandler.WriteServiceLog(GetUserId(), "Id:" + info.Id + ",projectName:" + info.projectName, "成功", "创建", "工程设置");
                    }
                    else
                    {
                        string ErrorCol = errors.Error;
                        LogHandler.WriteServiceLog(GetUserId(), "Id:" + info.Id + ",projectName:" + info.projectName + "," + ErrorCol, "失败", "创建", "工程设置");
                        return Json(JsonHandler.CreateMessage(0, Resource.InsertFail + ErrorCol), JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 删除
        [HttpPost]
        //[SupportFilter]
        public JsonResult Delete(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                if (m_BLL.m_Rep.Delete(Convert.ToInt32(id)) > 0)
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + id, "成功", "删除", "工程设置");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + id + "," + ErrorCol, "失败", "删除", "工程设置");
                    return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 工程详情跳转页
        public ActionResult Details(string ProjectId)
        {
            ViewBag.ProjectId = ProjectId;
            return View();
        }
        #endregion

        #region 工程详情
        public JsonResult GetDetails(string ProjectId)
        {

            LianTong_SystemCenterModel _LianTong_SystemCenterModel = m_BLL.m_Rep.Find(Convert.ToInt32(ProjectId));

            var json = new
            {
                Id = _LianTong_SystemCenterModel.Id,
                leaderName = _LianTong_SystemCenterModel.leaderName,
                projectName = _LianTong_SystemCenterModel.projectName,
                projectAttribution = _LianTong_SystemCenterModel.projectAttribution,
                projectArea = _LianTong_SystemCenterModel.projectArea,
                projectType = _LianTong_SystemCenterModel.projectType,
                AccountManager = _LianTong_SystemCenterModel.AccountManager,
                AccountManagerTel = _LianTong_SystemCenterModel.AccountManagerTel,
                SystemSupport = _LianTong_SystemCenterModel.SystemSupport,
                equipmentDealerContacts = _LianTong_SystemCenterModel.equipmentDealerContacts,
                equipmentDealerTel = _LianTong_SystemCenterModel.equipmentDealerTel,
                adress = _LianTong_SystemCenterModel.adress,
                contactPeople = _LianTong_SystemCenterModel.contactPeople,
                contactWay = _LianTong_SystemCenterModel.contactWay,
                equipmentType = _LianTong_SystemCenterModel.equipmentType,
                equipmentDealer = _LianTong_SystemCenterModel.equipmentDealer,
                constructionDepartment = _LianTong_SystemCenterModel.constructionDepartment,
                contractCost = _LianTong_SystemCenterModel.contractCost,
                equipmentCost = _LianTong_SystemCenterModel.equipmentCost,
                projectCost = _LianTong_SystemCenterModel.projectCost,
                Profit = m_BLL.getProfit(_LianTong_SystemCenterModel),
                ProfitRate = m_BLL.getProfitRate(_LianTong_SystemCenterModel),
                maintenanceDepartment = _LianTong_SystemCenterModel.maintenanceDepartment,
                maintenancePeriod = _LianTong_SystemCenterModel.maintenancePeriod,
                contractStartDate = _LianTong_SystemCenterModel.contractStartDate,
                contractEndDate = _LianTong_SystemCenterModel.contractEndDate,
                Comments = _LianTong_SystemCenterModel.Comments
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 上传页面
        public ActionResult UpLoad(string Id)
        {
            ViewBag.Id = Id;
            return View();
        }
        #endregion

        #region 上传并保存
        public JsonResult UpLoadSave(string Id, string FilePath)
        {
            var upmodel = m_BLL.m_Rep.Find(Convert.ToInt32(Id));
            upmodel.UpLoad = FilePath;
            if (m_BLL.m_Rep.Update(upmodel))
            {
                return Json(JsonHandler.CreateMessage(1, Resource.SetSucceed), JsonRequestBehavior.AllowGet);
            }
            else
            {
                string ErrorCol = errors.Error;
                return Json(JsonHandler.CreateMessage(0, Resource.SetFail), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 集成中心合同金额数统计
        [HttpPost]
        public ActionResult ContratcMoneyCnt()
        {
            return Json(m_BLL.ContratcMoneyCnt());
        }
        #endregion

        #region 导出Excel
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportExcel(string queryStr)
        {
            List<LianTong_SystemCenterModel> list;
            list = m_BLL.m_Rep.FindList(a => a.leaderName.Contains(queryStr) ||
            a.projectName.Contains(queryStr) ||
             a.projectAttribution.Contains(queryStr) ||
              a.AccountManager.Contains(queryStr) ||
               a.projectArea.Contains(queryStr) ||
                a.projectType.Contains(queryStr) ||
                 a.adress.Contains(queryStr) ||
                  a.contactWay.Contains(queryStr)
            ).ToList();
            return null;
        }
        #endregion

    }
}