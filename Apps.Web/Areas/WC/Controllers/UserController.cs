using System.Collections.Generic;
using System.Linq;
using Apps.Web.Core;

using Apps.Locale;
using System.Web.Mvc;
using Apps.Common;

using Apps.Models.WC;
using Microsoft.Practices.Unity;
using Senparc.Weixin.MP.AdvancedAPIs.User;
using System.Threading.Tasks;
using Apps.BLL.WC;
using Apps.Models;
using System;

namespace Apps.Web.Areas.WC.Controllers
{
    public class UserController : BaseController
    {

        public WC_UserBLL m_BLL = new WC_UserBLL();


        public WC_OfficalAccountsBLL account_BLL = new WC_OfficalAccountsBLL();


        public WC_GroupBLL group_BLL = new WC_GroupBLL();
        ValidationErrors errors = new ValidationErrors();

        ////[SupportFilter]
        public ActionResult Index()
        {
            WC_OfficalAccounts model = account_BLL.GetCurrentAccount();
            ViewBag.CurrentOfficalAcount = model.OfficalName;
            return View();
        }
        [HttpPost]
        ////[SupportFilter(ActionName = "Index")]
        public JsonResult GetList(GridPager pager, string queryStr)
        {
            List<WC_User> list = m_BLL.GetList(ref pager, queryStr);
            GridRows<WC_User> grs = new GridRows<WC_User>();
            grs.rows = list;
            grs.total = pager.totalRows;
            return Json(grs);
        }
        #region 创建
        ////[SupportFilter]
        public ActionResult Create()
        {
            ViewBag.Perm = GetPermission();
            return View();
        }

        [HttpPost]
        ////[SupportFilter]
        public JsonResult Create(WC_User model)
        {
            if (model != null)
            {

                if (m_BLL.m_Rep.Create(model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",OpenId" + model.OpenId, "成功", "创建", "WC_User");
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",OpenId" + model.OpenId + "," + ErrorCol, "失败", "创建", "WC_User");
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
        ////[SupportFilter]
        public ActionResult Edit(string id)
        {
            ViewBag.Perm = GetPermission();
            WC_User entity = m_BLL.m_Rep.Find(Convert.ToInt32(id));
            return View(entity);
        }

        [HttpPost]
        ////[SupportFilter]
        public JsonResult Edit(WC_User model)
        {
            if (model != null)
            {

                if (m_BLL.m_Rep.Update(model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",OpenId" + model.OpenId, "成功", "修改", "WC_User");
                    return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",OpenId" + model.OpenId + "," + ErrorCol, "失败", "修改", "WC_User");
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
        ////[SupportFilter]
        public ActionResult Details(string id)
        {
            ViewBag.Perm = GetPermission();
            WC_User entity = m_BLL.m_Rep.Find(Convert.ToInt32(id));
            return View(entity);
        }

        #endregion

        #region 删除
        [HttpPost]
        ////[SupportFilter]
        public JsonResult Delete(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                string[] arrs = id.Split(',');
                if (m_BLL.m_Rep.Delete(arrs)>0)
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Ids:" + id, "成功", "删除", "WC_User");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Ids" + id + "," + ErrorCol, "失败", "删除", "WC_User");
                    return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail));
            }


        }
        #endregion

        #region 同步
        [HttpPost]
        ////[SupportFilter]
        public JsonResult SyncUser(string id,string officeId)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                //填充数据
                string[] arrs = id.Split(',');
                List<BatchGetUserInfoData> list = new List<BatchGetUserInfoData>();
                foreach (var m in arrs)
                {
                    list.Add(new BatchGetUserInfoData() {
                        openid = m
                    });
                }

                //批量同步数据
                WC_OfficalAccounts accountModel =  account_BLL.m_Rep.Find(Convert.ToInt32(officeId));
                var batchList =  Senparc.Weixin.MP.AdvancedAPIs.UserApi.BatchGetUserInfo(accountModel.AccessToken, list);
                foreach (var info in batchList.user_info_list)
                {
                    WC_User userModel = m_BLL.m_Rep.Find(a => a.OpenId == info.openid);
                    if (userModel != null)
                    {
                        userModel.City = info.city;
                        userModel.OpenId = info.openid;
                        userModel.OpenId = info.openid;
                        userModel.HeadImgUrl = info.headimgurl;
                        userModel.Language = info.language;
                        userModel.NickName = info.nickname;
                        userModel.Province = info.province;
                        userModel.Sex = info.sex.ToString();
                        m_BLL.m_Rep.Update(userModel);
                    }
                }

                LogHandler.WriteServiceLog(GetUserId(), "Ids:" + id, "成功", "删除", "WC_User");
                return Json(JsonHandler.CreateMessage(1, Resource.SaveSucceed));
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.SaveFail));
            }


        }
        #endregion

        #region 批量移动用户分组 本地

        ////[SupportFilter(ActionName = "Edit")]
        public ActionResult MoveUser(string id)
        {
            ViewBag.UserIds = id.ToString();
            WC_OfficalAccounts model = account_BLL.GetCurrentAccount();
            List<WC_Group> list = group_BLL.m_Rep.FindList(a => a.OfficalAccountId == model.Id.ToString()).ToList();
            return View(list);
        }
        [HttpPost]
        ////[SupportFilter(ActionName = "Edit")]
        public ActionResult MoveUser(string userids, string groupid)
        {
            WC_OfficalAccounts wcmodel = account_BLL.GetCurrentAccount();
            List<string> itemstr = userids.Split(',').ToList();
            foreach (var item in itemstr)
            {
                ViewBag.Perm = GetPermission();
                WC_User model = m_BLL.m_Rep.Find(Convert.ToInt32(item));
                model.GroupId = groupid;
                if (m_BLL.m_Rep.Update(model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",OpenId" + model.OpenId, "成功", "修改", "WC_User");

                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",OpenId" + model.OpenId + "," + ErrorCol, "失败", "修改", "WC_User");
                    return Json(JsonHandler.CreateMessage(0, Resource.EditFail + ErrorCol));

                }
            }
            return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
        }

        #endregion
    }
}
