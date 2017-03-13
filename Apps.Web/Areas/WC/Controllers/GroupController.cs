using System.Collections.Generic;
using System.Linq;
using Apps.Web.Core;

using Apps.Locale;
using System.Web.Mvc;
using Apps.Common;

using Apps.Models.WC;
using Microsoft.Practices.Unity;
using Senparc.Weixin.MP.AdvancedAPIs.Groups;
using Senparc.Weixin.MP.AdvancedAPIs;
using Apps.Models;
using Apps.BLL.WC;
using System;

namespace Apps.Web.Areas.WC.Controllers
{
    public class GroupController : BaseController
    {

        public WC_GroupBLL m_BLL = new WC_GroupBLL();


        public WC_OfficalAccountsBLL account_BLL = new WC_OfficalAccountsBLL();
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
            List<WC_Group> list = m_BLL.GetList(ref pager, queryStr);
            GridRows<WC_Group> grs = new GridRows<WC_Group>();
            grs.rows = list;
            grs.total = pager.totalRows;
            return Json(grs);
        }


        #region 添加
        ////[SupportFilter]
        public ActionResult Create()
        {
            ViewBag.Perm = GetPermission();
            ViewBag.EditUrl = "Create";
            return View();
        }

        [HttpPost]
        ////[SupportFilter]
        public JsonResult Create(WC_Group model)
        {
            WC_OfficalAccounts wcmodel = account_BLL.GetCurrentAccount();
            model.OfficalAccountId = wcmodel.Id.ToString();
            model.Count = "0";
            if (model != null)
            {


                if (m_BLL.m_Rep.Create(model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name, "成功", "添加", "WC_Group");
                    return Json(JsonHandler.CreateMessage(1, Resource.Create));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name + "," + ErrorCol, "失败", "添加", "WC_Group");
                    return Json(JsonHandler.CreateMessage(0, Resource.Create + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.Create));
            }
        }
        #endregion

        #region 修改
        ////[SupportFilter]
        public ActionResult Edit(string id)
        {
            ViewBag.Perm = GetPermission();
            WC_Group entity = m_BLL.m_Rep.Find(Convert.ToInt32(id));
            ViewBag.EditUrl = "Edit";
            return View("Create", entity);
        }

        [HttpPost]
        ////[SupportFilter]
        public JsonResult Edit(WC_Group model)
        {
            if (model != null)
            {

                if (m_BLL.m_Rep.Update( model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name, "成功", "修改", "WC_Group");
                    return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name + "," + ErrorCol, "失败", "修改", "WC_Group");
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
            WC_Group entity = m_BLL.m_Rep.Find(Convert.ToInt32(id));
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
                if (m_BLL.m_Rep.Delete(Convert.ToInt32(id))> 0)
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + id, "成功", "删除", "WC_Group");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + id + "," + ErrorCol, "失败", "删除", "WC_Group");
                    return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail));
            }


        }
        #endregion

        #region 获取微信服务号分组信息
        [HttpPost]
        ////[SupportFilter(ActionName = "Index")]
        public JsonResult GetNetList()
        {

            WC_OfficalAccounts model = account_BLL.GetCurrentAccount();
            GroupsJson groupjson = GroupsApi.Get(model.AccessToken);
            foreach (var item in groupjson.groups)
            {
                WC_Group entity = new WC_Group();
                if (m_BLL.m_Rep.Find(item.id) == null)
                {
                    entity.Name = item.name;
                    entity.Count = item.count.ToString();
                    entity.OfficalAccountId = model.Id.ToString();
                    m_BLL.m_Rep.Create(entity);

                }

            }
            return Json(JsonHandler.CreateMessage(1, "获取成功"));
        }
        #endregion




        #region 更新到微信服务号分组信息
        [HttpPost]
        ////[SupportFilter(ActionName = "Edit")]
        public JsonResult UpdateNet()
        {

            WC_OfficalAccounts model = account_BLL.GetCurrentAccount();
            return Json(JsonHandler.CreateMessage(1, "更新成功"));
        }
        #endregion

    }
}
