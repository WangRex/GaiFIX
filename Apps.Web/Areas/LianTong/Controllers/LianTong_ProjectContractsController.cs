using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Apps.Common;
using Apps.Models;
using Microsoft.Practices.Unity;
using Apps.BLL;
using Apps.Models.Sys;
using System;
using Apps.Web.Core;
using Apps.Locale;
using Apps.BLL.Sys;
using System.Data.SqlClient;
using Apps.BLL.LianTong;
using Apps.Models.LianTong;
using Newtonsoft.Json;

namespace Apps.Web.Areas.LianTong.Controllers
{
    public class LianTong_ProjectContractsController : BaseController
    {

        private LianTong_ProjectBLL _LianTong_Project = new LianTong_ProjectBLL();
        private LianTong_ProjectContractsBLL m_BLL = new LianTong_ProjectContractsBLL();
        private SysStructBLL StructBLL = new SysStructBLL();
        ValidationErrors errors = new ValidationErrors();

        [SupportFilter]
        public ActionResult Index()
        {
            return View();
        }

        #region 获取合同列表
        //[SupportFilter(ActionName = "Index")]
        public JsonResult GetList(GridPager pager, string queryStr)
        {
            if (queryStr == null) queryStr = string.Empty;
            string DepId = GetAccount().DepId;
            string QuaryCD = StructBLL.m_Rep.Find(Convert.ToInt32(DepId)).Remark;
            string RoleName = GetAccount().RoleName;
            List<LianTong_ProjectContractsModel> list;
            if ("[超级管理员]".Equals(RoleName))
            {
                list = m_BLL.m_Rep.FindPageList(ref pager, a => string.IsNullOrEmpty(a.history)).ToList();
            }
            else if ("[公司领导]".Equals(RoleName) || RoleName.Contains("[工程管理部]"))
            {
                list = m_BLL.m_Rep.FindPageList(ref pager, a => a.department.Equals(DepId) && (a.history == null || a.history == string.Empty)).ToList();
            }
            else
            {
                list = m_BLL.m_Rep.FindPageList(ref pager, a => a.department == DepId && (a.history == null || a.history == string.Empty)).ToList();
            }


            var json = new
            {
                total = pager.totalRows,
                rows = list
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 分配工程页面跳转
        [SupportFilter(ActionName = "Index")]
        public ActionResult ProjectsBindingPage(string ContractsId)
        {
            ViewBag.ContractsId = ContractsId;
            return View();
        }
        #endregion

        #region 获取待关联工程集合
        //[SupportFilter(ActionName = "Allot")]
        public JsonResult GetTargetProjects()
        {
            string DepId = GetAccount().DepId;
            string QuaryCD = StructBLL.m_Rep.Find(Convert.ToInt32(DepId)).Remark;
            string RoleName = GetAccount().RoleName;
            List<LianTong_ProjectModel> list;
            if ("[超级管理员]".Equals(RoleName))
            {
                list = _LianTong_Project.m_Rep.FindList(a => a.contractNum == null).ToList();
            }
            else
            {
                list = _LianTong_Project.m_Rep.FindList(a => a.department == DepId && (a.contractNum == null || a.contractNum == string.Empty)).ToList();
            }
            var jsonData = new
            {
                total = list.Count(),
                rows = (
                    from r in list
                    select new
                    {
                        Id = r.Id,
                        projectNum = r.projectNum,
                        singleProjectName = r.singleProjectName,
                        outlineCost = r.outlineCost,
                        laborCost = r.laborCost,
                        materialsCost = r.materialsCost,
                        departmentName = r.departmentName,
                        Flag = String.IsNullOrEmpty(r.contractNum) ? "0" : "1"
                    }
                ).ToArray()
            };
            return Json(jsonData);
        }
        #endregion

        #region 关联工程
        //[SupportFilter(ActionName = "Save")]
        public JsonResult BindingProject(string ContractsId, string projectIds)
        {
            if (m_BLL.BindingProject(ContractsId, projectIds))
            {
                LogHandler.WriteServiceLog(GetUserId(), "projectNums:" + projectIds, "成功", "分配工程", "合同设置");
                return Json(JsonHandler.CreateMessage(1, Resource.SetSucceed), JsonRequestBehavior.AllowGet);
            }
            else
            {
                string ErrorCol = errors.Error;
                LogHandler.WriteServiceLog(GetUserId(), "projectNums:" + projectIds, "失败", "分配工程", "合同设置");
                return Json(JsonHandler.CreateMessage(0, Resource.SetFail), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 创建
        [HttpPost]
        public ActionResult CreatByGrid()
        {
            string result = Request.Form[0];

            //后台拿到字符串时直接反序列化。根据需要自己处理
            List<LianTong_ProjectContractsModel> datagridList = new List<LianTong_ProjectContractsModel>();
            try
            { datagridList = JsonConvert.DeserializeObject<List<LianTong_ProjectContractsModel>>(result); }
            catch (Exception)
            {
                string ErrorCol = "输入数据类型错误，请点撤销后重新输入";
                LogHandler.WriteServiceLog(GetUserId(), ErrorCol, "失败", "数据更新", "合同设置");
                return Json(JsonHandler.CreateMessage(0, Resource.SetFail), JsonRequestBehavior.AllowGet);
            }
            foreach (LianTong_ProjectContractsModel info in datagridList)
            {
                if (info.Id > 0)
                {
                    string departmentName = StructBLL.m_Rep.Find(Convert.ToInt32(info.department)).Name;
                    info.departmentName = departmentName;
                    if (m_BLL.m_Rep.Update(info))
                    {
                        LogHandler.WriteServiceLog(GetUserId(), "Id:" + info.Id + ",contractNum:" + info.contractNum, "成功", "修改", "合同设置");
                    }
                    else
                    {
                        string ErrorCol = errors.Error;
                        LogHandler.WriteServiceLog(GetUserId(), "Id:" + info.Id + ",contractNum:" + info.contractNum + "," + ErrorCol, "失败", "修改", "合同设置");
                        return Json(JsonHandler.CreateMessage(0, Resource.EditFail + ":" + ErrorCol));
                    }
                }
                else
                {
                    info.departmentName = StructBLL.m_Rep.Find(Convert.ToInt32(info.department)).Name;
                    if (m_BLL.m_Rep.Create(info))
                    {
                        LogHandler.WriteServiceLog(GetUserId(), "Id:" + info.Id + ",contractNum:" + info.contractNum, "成功", "创建", "合同设置");
                    }
                    else
                    {
                        string ErrorCol = errors.Error;
                        LogHandler.WriteServiceLog(GetUserId(), "Id:" + info.Id + ",contractNum:" + info.contractNum + "," + ErrorCol, "失败", "创建", "合同设置");
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
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + id, "成功", "删除", "合同设置");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + id + "," + ErrorCol, "失败", "删除", "合同设置");
                    return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 合同数统计
        [HttpPost]
        public ActionResult contractsCNTJsonList()
        {
            Account _Account = (Account)Session["Account"];
            string roles = _Account.RoleName;
            string DepId = _Account.DepId;
            if (null == roles || roles.Equals("[超级管理员]") || roles.Equals("[公司领导]") || roles.Contains("[工程管理部]"))
            {
                DepId = string.Empty;
            }
            return Json(m_BLL.ContractsCnt(DepId));
        }
        #endregion

        #region 概算费
        [HttpPost]
        public ActionResult outlineCostReport()
        {
            return Json(m_BLL.outlineCostReport());
        }
        #endregion

    }
}