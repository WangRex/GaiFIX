using System.Collections.Generic;
using System.Linq;
using Apps.Web.Core;

using Apps.Locale;
using System.Web.Mvc;
using Apps.Common;

using Apps.Models.WC;
using Microsoft.Practices.Unity;
using Apps.BLL.WC;
using System;

namespace Apps.Web.Areas.WC.Controllers
{
    public class OfficalAccountsController : BaseController
    {

        public WC_OfficalAccountsBLL m_BLL = new WC_OfficalAccountsBLL();
        ValidationErrors errors = new ValidationErrors();

        ////[SupportFilter]
        public ActionResult Index()
        {
            WC_OfficalAccounts model = m_BLL.GetCurrentAccount();
            ViewBag.CurrentOfficalAcount = model.OfficalName;
            return View();
        }
        [HttpPost]
        ////[SupportFilter(ActionName = "Index")]
        public JsonResult GetList(GridPager pager, string queryStr)
        {
            List<WC_OfficalAccounts> list = m_BLL.GetList(ref pager, queryStr);
            GridRows<WC_OfficalAccounts> grs = new GridRows<WC_OfficalAccounts>();
            grs.rows = list;
            grs.total = pager.totalRows;
            return Json(grs);
        }
        [HttpPost]
        ////[SupportFilter(ActionName = "Edit")]
        public JsonResult SetDefault(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                if (m_BLL.SetDefault(Convert.ToInt32(id)))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + id, "成功", "设置默认", "WC_OfficalAccounts");
                    return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + id, "失败", "设置默认", "WC_OfficalAccounts");
                    return Json(JsonHandler.CreateMessage(0, Resource.EditFail + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.EditFail));
            }
        }

        #region 创建
        ////[SupportFilter]
        public ActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        ////[SupportFilter]
        public JsonResult Create(WC_OfficalAccounts model)
        {
          
            if (model != null)
            {
                model.CreateTime = ResultHelper.NowTime.ToString("yyyy-MM-dd HH:mm:ss");
                model.CreateBy = GetUserId();
                model.ModifyTime = ResultHelper.NowTime.ToString("yyyy-MM-dd HH:mm:ss");
                model.ModifyBy = GetUserId();
                if (m_BLL.m_Rep.Create(model))
                {
                    model.ApiUrl = WebChatPara.ApiUrl + model.Id;
                    m_BLL.m_Rep.Update(model);
                    if (model.IsDefault == "true")
                    {
                        m_BLL.SetDefault(model.Id);
                    }
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",OfficalName" + model.OfficalName, "成功", "创建", "WC_OfficalAccounts");
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",OfficalName" + model.OfficalName + "," + ErrorCol, "失败", "创建", "WC_OfficalAccounts");
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
            
            WC_OfficalAccounts entity = m_BLL.m_Rep.Find(Convert.ToInt32(id));
            return View(entity);
        }

        [HttpPost]
        ////[SupportFilter]
        public JsonResult Edit(WC_OfficalAccounts model)
        {
            if (model != null)
            {
                model.ModifyTime = ResultHelper.NowTime.ToString("yyyy-MM-dd HH:mm:ss");
                model.ModifyBy = GetUserId();
                model.ApiUrl = WebChatPara.ApiUrl + model.Id;
                if (m_BLL.m_Rep.Update (model))
                {
                    if (model.IsDefault == "true")
                    {
                        m_BLL.SetDefault(model.Id);
                    }
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",OfficalName" + model.OfficalName, "成功", "修改", "WC_OfficalAccounts");
                    return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",OfficalName" + model.OfficalName + "," + ErrorCol, "失败", "修改", "WC_OfficalAccounts");
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
            
            WC_OfficalAccounts entity = m_BLL.m_Rep.Find(Convert.ToInt32(id));
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
                WC_OfficalAccounts model = m_BLL.m_Rep.Find(Convert.ToInt32(id));
                if (model.IsDefault == "true")
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + id + "删除失败，因为当前公众号为默认公众号", "失败", "删除", "WC_OfficalAccounts");
                    return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail + ",因为当前公众号为默认公众号"));
                }

                if (m_BLL.m_Rep.Delete(model)>0)
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + id, "成功", "删除", "WC_OfficalAccounts");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + id + "," + ErrorCol, "失败", "删除", "WC_OfficalAccounts");
                    return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail));
            }


        }
        #endregion
        [HttpPost]
        public JsonResult GetToken()
        {

            List<WC_OfficalAccounts> list = m_BLL.GetList(ref setNoPagerAscById, "");
            foreach (var model in list)
            {
                if (!string.IsNullOrEmpty(model.AppId) && !string.IsNullOrEmpty(model.AppSecret))
                {
                    model.AccessToken = Senparc.Weixin.MP.CommonAPIs.CommonApi.GetToken(model.AppId, model.AppSecret).access_token;
                    model.ModifyTime = ResultHelper.NowTime.ToString("yyyy-MM-dd HH:mm:ss");
                    m_BLL.m_Rep.Update(model);
                }
            }

            return Json(JsonHandler.CreateMessage(1, "成批更新成功"));
        }
    }
}
