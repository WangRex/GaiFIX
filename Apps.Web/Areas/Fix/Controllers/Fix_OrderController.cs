using Apps.BLL.Fix;
using Apps.BLL.Sys;
using Apps.Common;
using Apps.Locale;
using Apps.Models.FIX;
using Apps.Models.Sys;
using Apps.Web.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Apps.Web.Areas.Fix.Controllers
{
    /// <summary>
    /// 订单管理
    /// </summary>
    public class Fix_OrderController : BaseController
    {
        private Fix_OrderBLL m_BLL = new Fix_OrderBLL();
        private SysStructBLL StructBLL = new SysStructBLL();
        private Fix_AssessmentBLL a_BLL = new Fix_AssessmentBLL();
        private Fix_MatchingBLL ma_BLL = new Fix_MatchingBLL();
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

        #region 获取订单列表
        //[SupportFilter(ActionName = "Index")]
        public JsonResult GetList(GridPager pager, string queryDepId, string StatusValue)
        {
            string DepId = GetAccount().DepId;
            string QuaryCD = StructBLL.m_Rep.Find(Convert.ToInt32(DepId)).Remark;
            string RoleName = GetAccount().RoleName;
            List<FIX_OrderModel> list;
            if ("[超级管理员]".Equals(RoleName))
            {
                list = m_BLL.m_Rep.FindPageList(ref pager, o => (string.IsNullOrEmpty(queryDepId) || o.department == queryDepId) && (string.IsNullOrEmpty(StatusValue) || o.Status == StatusValue)).ToList();
            }
            else
            {
                list = m_BLL.m_Rep.FindPageList(ref pager, o => (string.IsNullOrEmpty(DepId) || o.department == DepId) && (string.IsNullOrEmpty(StatusValue) || o.Status == StatusValue)).ToList();
            }
            var json = new
            {
                total = pager.totalRows,
                rows = list
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 创建
        [HttpPost]
        public ActionResult CreatByGrid()
        {
            string result = Request.Form[0];

            //后台拿到字符串时直接反序列化。根据需要自己处理
            List<FIX_OrderModel> datagridList = new List<FIX_OrderModel>();
            try
            { datagridList = JsonConvert.DeserializeObject<List<FIX_OrderModel>>(result); }
            catch (Exception)
            {
                string ErrorCol = "输入数据类型错误，请点撤销后重新输入";
                LogHandler.WriteServiceLog(GetUserId(), ErrorCol, "失败", "数据更新", "订单");
                return Json(JsonHandler.CreateMessage(0, Resource.SetFail), JsonRequestBehavior.AllowGet);
            }
            foreach (FIX_OrderModel info in datagridList)
            {
                if (info.Id > 0)
                {
                    string departmentName = StructBLL.m_Rep.Find(Convert.ToInt32(info.department)).Name;
                    info.departmentName = departmentName;
                    info.UpdatePerson = GetAccount().TrueName;
                    info.UpdateTime = DateTime.Now.ToShortDateString();
                    if (!string.IsNullOrEmpty(info.MatchingAddress))
                    {
                        UserOTM _UserOTM = ma_BLL.getOTM(info.MatchingAddress);
                        if (null != _UserOTM)
                        {
                            info.OTM_ID = _UserOTM.Id;
                            info.OTM_Name = _UserOTM.UserName;
                        }
                    }
                    if (m_BLL.m_Rep.Update(info))
                    {
                        LogHandler.WriteServiceLog(GetUserId(), "Id:" + info.Id + ",Name:" + info.Name, "成功", "修改", "订单");
                    }
                    else
                    {
                        string ErrorCol = errors.Error;
                        LogHandler.WriteServiceLog(GetUserId(), "Id:" + info.Id + ",Name:" + info.Name + "," + ErrorCol, "失败", "修改", "订单");
                        return Json(JsonHandler.CreateMessage(0, Resource.EditFail + ":" + ErrorCol));
                    }
                }
                else
                {
                    info.departmentName = StructBLL.m_Rep.Find(Convert.ToInt32(info.department)).Name;
                    info.CreatePerson = GetAccount().TrueName;
                    info.CreateTime = DateTime.Now.ToShortDateString();
                    info.OTM_ID = "0";
                    info.OTM_Name = "暂无外线员";
                    if (!string.IsNullOrEmpty(info.MatchingAddress))
                    {
                        UserOTM _UserOTM = ma_BLL.getOTM(info.MatchingAddress);
                        if (null != _UserOTM)
                        {
                            info.OTM_ID = _UserOTM.Id;
                            info.OTM_Name = _UserOTM.UserName;
                            info.Status = "待接单";
                        }
                    }
                    if (m_BLL.m_Rep.Create(info))
                    {
                        LogHandler.WriteServiceLog(GetUserId(), "Id:" + info.Id + ",Name:" + info.Name, "成功", "创建", "订单");
                    }
                    else
                    {
                        string ErrorCol = errors.Error;
                        LogHandler.WriteServiceLog(GetUserId(), "Id:" + info.Id + ",Name:" + info.Name + "," + ErrorCol, "失败", "创建", "订单");
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
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + id, "成功", "删除", "订单");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + id + "," + ErrorCol, "失败", "删除", "订单");
                    return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 订单详情跳转页
        public ActionResult Details(string Id)
        {
            ViewBag.Id = Id;
            return View();
        }
        #endregion

        #region 订单详情
        public JsonResult GetDetails(string Id)
        {

            FIX_OrderModel _FIX_OrderModel = m_BLL.m_Rep.Find(Convert.ToInt32(Id));
            FIX_AssessmentModel _FIX_AssessmentModel = a_BLL.getAssessment(_FIX_OrderModel.Id);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", _FIX_OrderModel.Id);
            dic.Add("Name", _FIX_OrderModel.Name);
            dic.Add("Province", _FIX_OrderModel.Province);
            dic.Add("City", _FIX_OrderModel.City);
            dic.Add("Area", _FIX_OrderModel.Area);
            dic.Add("Time", _FIX_OrderModel.Time);
            dic.Add("Address", _FIX_OrderModel.Address);
            dic.Add("MatchingAddress", _FIX_OrderModel.MatchingAddress);
            dic.Add("Phone", _FIX_OrderModel.Phone);
            dic.Add("Tel", _FIX_OrderModel.Tel);
            dic.Add("Type", _FIX_OrderModel.Type);
            dic.Add("Status", _FIX_OrderModel.Status);
            dic.Add("Description", _FIX_OrderModel.Description);
            dic.Add("OTM_Name", _FIX_OrderModel.OTM_Name);
            dic.Add("departmentName", _FIX_OrderModel.departmentName);
            dic.Add("Content", null == _FIX_AssessmentModel ? "" : _FIX_AssessmentModel.Content);
            dic.Add("ComprehensiveStar", null == _FIX_AssessmentModel ? "" : _FIX_AssessmentModel.ComprehensiveStar.ToString());
            dic.Add("QuestionStar", null == _FIX_AssessmentModel ? "" : _FIX_AssessmentModel.QuestionStar.ToString());
            dic.Add("AttitudeStar", null == _FIX_AssessmentModel ? "" : _FIX_AssessmentModel.AttitudeStar.ToString());
            return Json(dic, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}