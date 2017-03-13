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
using System.Linq.Expressions;

namespace Apps.Web.Areas.LianTong.Controllers
{
    public class LianTong_ProjectController : BaseController
    {
        private LianTong_ProjectContractsBLL contractsBLL = new LianTong_ProjectContractsBLL();
        private LianTong_ProjectBLL m_BLL = new LianTong_ProjectBLL();
        private SysStructBLL StructBLL = new SysStructBLL();
        ValidationErrors errors = new ValidationErrors();
        [SupportFilter]
        public ActionResult Index()
        {
            return View();
        }
        //[SupportFilter(ActionName = "Index")]
        public JsonResult GetList(GridPager pager, string DepId, string overStatus, string projectPro, string admin, string projectManagement)
        {
            string UserDepId = GetAccount().DepId;
            string QuaryCD = StructBLL.m_Rep.Find(Convert.ToInt32(UserDepId)).Remark;
            string RoleName = GetAccount().RoleName;
            List<LianTong_ProjectModel> list;
            if ("[超级管理员]".Equals(RoleName) || "[公司领导]".Equals(RoleName))
            {
                list = m_BLL.m_Rep.FindPageList(ref pager, p => (string.IsNullOrEmpty(DepId) || p.department == DepId) && (string.IsNullOrEmpty(overStatus) || p.overStatus == overStatus) && (string.IsNullOrEmpty(projectPro) || p.projectPro.Contains(projectPro)) && (string.IsNullOrEmpty(admin) || p.admin.Contains(admin)) && (string.IsNullOrEmpty(projectManagement) || p.department.Contains(projectManagement))).ToList();
            }
            else
            {
                list = m_BLL.m_Rep.FindPageList(ref pager, p => (p.department.Equals(UserDepId)) && (p.contractNum.Equals(string.Empty) || p.contractNum.Equals(null)) && (string.IsNullOrEmpty(DepId) || p.department == DepId) && (string.IsNullOrEmpty(overStatus) || p.overStatus == overStatus) && (string.IsNullOrEmpty(projectPro) || p.projectPro.Contains(projectPro)) && (string.IsNullOrEmpty(admin) || p.admin.Contains(admin)) && (string.IsNullOrEmpty(projectManagement) || p.department.Contains(projectManagement))).ToList();
            }

            var json = new
            {
                total = pager.totalRows,
                rows = list
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreatByGrid()
        {
            string result = Request.Form[0];

            //后台拿到字符串时直接反序列化。根据需要自己处理
            //var datagrid = JsonConvert.DeserializeObject<List<datagrid>>(result);
            List<LianTong_ProjectModel> datagridList = new List<LianTong_ProjectModel>();
            try
            { datagridList = JsonConvert.DeserializeObject<List<LianTong_ProjectModel>>(result); }
            catch (Exception)
            {
                string ErrorCol = "输入数据类型错误，请点撤销后重新输入";
                LogHandler.WriteServiceLog(GetUserId(), ErrorCol, "失败", "数据更新", "工程设置");
                return Json(JsonHandler.CreateMessage(0, Resource.SetFail), JsonRequestBehavior.AllowGet);
            }
            foreach (LianTong_ProjectModel info in datagridList)
            {
                if (info.Id > 0)
                {
                    if (m_BLL.m_Rep.Update(info))
                    {
                        LogHandler.WriteServiceLog(GetUserId(), "Id:" + info.Id + ",contractNum:" + info.contractNum, "成功", "修改", "工程设置");
                    }
                    else
                    {
                        string ErrorCol = errors.Error;
                        LogHandler.WriteServiceLog(GetUserId(), "Id:" + info.Id + ",contractNum:" + info.contractNum + "," + ErrorCol, "失败", "修改", "工程设置");
                        return Json(JsonHandler.CreateMessage(0, Resource.EditFail + ":" + ErrorCol));
                    }
                }
                else
                {
                    info.department = GetAccount().DepId;
                    info.departmentName = StructBLL.m_Rep.Find(Convert.ToInt32 (info.department)).Name;
                    string startNum = info.department + DateTime.Now.ToString("yyyyMMdd_hhmmss").Substring(2, 6);
                    info.projectNum = startNum + m_BLL.GetProjectChildNum(startNum);
                    if (m_BLL.m_Rep.Create(info))
                    {
                        LogHandler.WriteServiceLog(GetUserId(), "Id:" + info.Id + ",contractNum:" + info.contractNum, "成功", "创建", "工程设置");
                    }
                    else
                    {
                        string ErrorCol = errors.Error;
                        LogHandler.WriteServiceLog(GetUserId(), "Id:" + info.Id + ",contractNum:" + info.contractNum + "," + ErrorCol, "失败", "创建", "工程设置");
                        return Json(JsonHandler.CreateMessage(0, Resource.InsertFail + ErrorCol),JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed), JsonRequestBehavior.AllowGet);
        }

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

        public ActionResult Details(string ContractsId)
        {
            ViewBag.ContractsId = ContractsId;
            return View();
        }

        //[SupportFilter(ActionName = "Allot")]
        public JsonResult GetTargetProjects(string ContractsId)
        {
            var contracts = contractsBLL.m_Rep.Find(Convert.ToInt32(ContractsId));
            List<LianTong_ProjectModel> list = m_BLL.m_Rep.FindList(a => a.contractNum == contracts.contractNum).ToList();
            return Json(list);
        }
        

    }
}