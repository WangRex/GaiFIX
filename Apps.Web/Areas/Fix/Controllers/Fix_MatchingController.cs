using Apps.BLL.Fix;
using Apps.BLL.Sys;
using Apps.Common;
using Apps.Locale;
using Apps.Models.FIX;
using Apps.Web.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Apps.Web.Areas.Fix.Controllers
{
    /// <summary>
    /// 匹配管理
    /// </summary>
    public class Fix_MatchingController : BaseController
    {
        private Fix_MatchingBLL m_BLL = new Fix_MatchingBLL();
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

        #region 获取地址列表
        //[SupportFilter(ActionName = "Index")]
        public JsonResult GetList(GridPager pager, string queryDepId, string StatusValue)
        {
            string DepId = GetAccount().DepId;
            string QuaryCD = StructBLL.m_Rep.Find(Convert.ToInt32(DepId)).Remark;
            string RoleName = GetAccount().RoleName;
            List<FIX_MatchingModel> list;
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
            List<FIX_MatchingModel> datagridList = new List<FIX_MatchingModel>();
            try
            { datagridList = JsonConvert.DeserializeObject<List<FIX_MatchingModel>>(result); }
            catch (Exception)
            {
                string ErrorCol = "输入数据类型错误，请点撤销后重新输入";
                LogHandler.WriteServiceLog(GetUserId(), ErrorCol, "失败", "数据更新", "匹配地址");
                return Json(JsonHandler.CreateMessage(0, Resource.SetFail), JsonRequestBehavior.AllowGet);
            }
            foreach (FIX_MatchingModel info in datagridList)
            {
                if (info.Id > 0)
                {
                    string departmentName = StructBLL.m_Rep.Find(Convert.ToInt32(info.department)).Name;
                    info.departmentName = departmentName;
                    info.UpdatePerson = GetAccount().TrueName;
                    info.UpdateTime = DateTime.Now.ToShortDateString();
                    if (m_BLL.m_Rep.Update(info))
                    {
                        LogHandler.WriteServiceLog(GetUserId(), "Id:" + info.Id + ",Address:" + info.Address, "成功", "修改", "匹配地址");
                    }
                    else
                    {
                        string ErrorCol = errors.Error;
                        LogHandler.WriteServiceLog(GetUserId(), "Id:" + info.Id + ",Address:" + info.Address + "," + ErrorCol, "失败", "修改", "匹配地址");
                        return Json(JsonHandler.CreateMessage(0, Resource.EditFail + ":" + ErrorCol));
                    }
                }
                else
                {
                    info.departmentName = StructBLL.m_Rep.Find(Convert.ToInt32(info.department)).Name;
                    info.CreatePerson = GetAccount().TrueName;
                    info.CreateTime = DateTime.Now.ToShortDateString();
                    if (m_BLL.m_Rep.Create(info))
                    {
                        LogHandler.WriteServiceLog(GetUserId(), "Id:" + info.Id + ",Address:" + info.Address, "成功", "创建", "匹配地址");
                    }
                    else
                    {
                        string ErrorCol = errors.Error;
                        LogHandler.WriteServiceLog(GetUserId(), "Id:" + info.Id + ",Address:" + info.Address + "," + ErrorCol, "失败", "创建", "匹配地址");
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
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + id, "成功", "删除", "匹配地址");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + id + "," + ErrorCol, "失败", "删除", "匹配地址");
                    return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 设置地址外线员
        public ActionResult UserBindingPage(string matchingId)
        {
            ViewBag.matchingId = matchingId;
            ViewBag.StructTree = StructBLL.GetStructTree(true);
            return View();
        }
        #endregion

        #region 获取用户
        public JsonResult GetUserListByMatching(GridPager pager, string matchingId, string depId, string queryStr)
        {
            if (string.IsNullOrWhiteSpace(matchingId))
            {
                return Json(0);
            }
            var userList = m_BLL.GetMatchingListByUser(matchingId, depId, queryStr);
            var jsonData = new
            {
                total = pager.totalRows,
                rows = (
                    from r in userList
                    select new
                    {
                        Id = r.Id,
                        UserName = r.UserName,
                        TrueName = r.TrueName,
                        Flag = r.Flag == "0" ? "0" : "1",
                    }
                ).ToArray()
            };
            return Json(jsonData);
        }
        #endregion

        #region 关联外线员
        public JsonResult BindingUser(string matchingId, string userId)
        {
            if (m_BLL.BindingUser(matchingId, userId, GetAccount().TrueName))
            {
                LogHandler.WriteServiceLog(GetUserId(), "matchingId:" + matchingId, "成功", "关联", "关联外线员");
                return Json(JsonHandler.CreateMessage(1, Resource.SetSucceed), JsonRequestBehavior.AllowGet);
            }
            else
            {
                string ErrorCol = errors.Error;
                LogHandler.WriteServiceLog(GetUserId(), "matchingId:" + matchingId, "失败", "关联", "关联外线员");
                return Json(JsonHandler.CreateMessage(0, Resource.SetFail), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}