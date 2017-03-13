using Apps.BLL.Fix;
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
    /// 客户数据
    /// </summary>
    public class Fix_CustomerController : BaseController
    {
        private Fix_CustomerBLL m_BLL = new Fix_CustomerBLL();
        ValidationErrors errors = new ValidationErrors();

        /// <summary>
        /// 首页跳转
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        #region 获取客户列表
        //[SupportFilter(ActionName = "Index")]
        public JsonResult GetList(GridPager pager, string queryStr)
        {
            string DepId = GetAccount().DepId;
            string RoleName = GetAccount().RoleName;
            List<FIX_CustomerModel> list;
            if ("[超级管理员]".Equals(RoleName))
            {
                list = m_BLL.m_Rep.FindPageList(ref pager, c => (string.IsNullOrEmpty(queryStr) || c.Name == queryStr)).ToList();
            }
            else
            {
                list = m_BLL.m_Rep.FindPageList(ref pager, c => (string.IsNullOrEmpty(queryStr) || c.Name == queryStr)).ToList();
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
            List<FIX_CustomerModel> datagridList = new List<FIX_CustomerModel>();
            try
            { datagridList = JsonConvert.DeserializeObject<List<FIX_CustomerModel>>(result); }
            catch (Exception)
            {
                string ErrorCol = "输入数据类型错误，请点撤销后重新输入";
                LogHandler.WriteServiceLog(GetUserId(), ErrorCol, "失败", "数据更新", "客户数据");
                return Json(JsonHandler.CreateMessage(0, Resource.SetFail), JsonRequestBehavior.AllowGet);
            }
            foreach (FIX_CustomerModel info in datagridList)
            {
                if (info.Id > 0)
                {
                    info.UpdatePerson = GetAccount().TrueName;
                    info.UpdateTime = DateTime.Now.ToShortDateString();
                    if (m_BLL.m_Rep.Update(info))
                    {
                        LogHandler.WriteServiceLog(GetUserId(), "Id:" + info.Id + ",Address:" + info.Address, "成功", "修改", "客户数据");
                    }
                    else
                    {
                        string ErrorCol = errors.Error;
                        LogHandler.WriteServiceLog(GetUserId(), "Id:" + info.Id + ",Address:" + info.Address + "," + ErrorCol, "失败", "修改", "客户数据");
                        return Json(JsonHandler.CreateMessage(0, Resource.EditFail + ":" + ErrorCol));
                    }
                }
                else
                {
                    info.CreatePerson = GetAccount().TrueName;
                    info.CreateTime = DateTime.Now.ToShortDateString();
                    if (m_BLL.m_Rep.Create(info))
                    {
                        LogHandler.WriteServiceLog(GetUserId(), "Id:" + info.Id + ",Address:" + info.Address, "成功", "创建", "客户数据");
                    }
                    else
                    {
                        string ErrorCol = errors.Error;
                        LogHandler.WriteServiceLog(GetUserId(), "Id:" + info.Id + ",Address:" + info.Address + "," + ErrorCol, "失败", "创建", "客户数据");
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

        #region 导入
        [HttpPost]
        //[SupportFilter]
        public ActionResult Import(string filePath)
        {
            var customerList = new List<FIX_CustomerModel>();
            bool checkResult = false;
            //校验数据
            checkResult = m_BLL.CheckImportData(Utils.GetMapPath(filePath), customerList, ref errors);
            //校验通过直接保存
            if (checkResult)
            {
                m_BLL.SaveImportData(customerList);
                LogHandler.WriteServiceLog(GetUserId(), "导入成功", "成功", "导入", "Spl_Person");
                return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
            }
            else
            {
                string ErrorCol = errors.Error;
                LogHandler.WriteServiceLog(GetUserId(), ErrorCol, "失败", "导入", "Spl_Person");
                return Json(JsonHandler.CreateMessage(0, Resource.InsertFail + ErrorCol));
            }
        }
        #endregion
    }
}