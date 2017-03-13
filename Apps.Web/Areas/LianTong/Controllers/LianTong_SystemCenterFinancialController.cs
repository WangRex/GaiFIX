using Apps.BLL.LianTong;
using Apps.Common;
using Apps.Locale;
using Apps.Models.LianTong;
using Apps.Web.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Apps.Web.Areas.LianTong.Controllers
{
    public class LianTong_SystemCenterFinancialController : BaseController
    {
        private LianTong_SystemCenterFinancialBLL m_BLL = new LianTong_SystemCenterFinancialBLL();
        ValidationErrors errors = new ValidationErrors();

        [SupportFilter]
        public ActionResult Index()
        {
            return View();
        }

        #region 获取财务列表
        public JsonResult GetList(GridPager pager, string queryStr)
        {
            if (string.IsNullOrEmpty(queryStr)) queryStr = "";
            List<LianTong_SystemCenterFinancialModel> list;
            list = m_BLL.m_Rep.FindPageList(ref pager, a => a.Type.Contains(queryStr) ||
            a.Status.Contains(queryStr) ||
             a.Operator.Contains(queryStr) ||
              a.Source.Contains(queryStr) ||
               a.Description.Contains(queryStr)
            ).ToList();

            var json = new
            {
                total = pager.totalRows,
                rows = (from r in list
                        select new
                        {
                            Id = r.Id,
                            Date = r.Date,
                            Type = r.Type,
                            Status = r.Status,
                            Operator = r.Operator,
                            Money = r.Money,
                            Source = r.Source,
                            Description = r.Description,
                            Invoice = r.Invoice,
                            Remark = r.Remark
                        }).ToArray()
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 保存财务数据
        [HttpPost]
        public ActionResult CreatByGrid()
        {
            string result = Request.Form[0];

            //后台拿到字符串时直接反序列化。根据需要自己处理
            List<LianTong_SystemCenterFinancialModel> datagridList = new List<LianTong_SystemCenterFinancialModel>();
            try
            { datagridList = JsonConvert.DeserializeObject<List<LianTong_SystemCenterFinancialModel>>(result); }
            catch (Exception)
            {
                string ErrorCol = "输入数据类型错误，请点撤销后重新输入";
                LogHandler.WriteServiceLog(GetUserId(), ErrorCol, "失败", "数据更新", "财务数据");
                return Json(JsonHandler.CreateMessage(0, Resource.SetFail), JsonRequestBehavior.AllowGet);
            }
            foreach (LianTong_SystemCenterFinancialModel info in datagridList)
            {
                if (info.Id > 0)
                {
                    if (m_BLL.m_Rep.Update(info))
                    {
                        LogHandler.WriteServiceLog(GetUserId(), "Id:" + info.Id + ",Operator:" + info.Operator, "成功", "修改", "财务数据");
                    }
                    else
                    {
                        string ErrorCol = errors.Error;
                        LogHandler.WriteServiceLog(GetUserId(), "Id:" + info.Id + ",Operator:" + info.Operator + "," + ErrorCol, "失败", "修改", "财务数据");
                        return Json(JsonHandler.CreateMessage(0, Resource.EditFail + ":" + ErrorCol));
                    }
                }
                else
                {
                    if (m_BLL.m_Rep.Create(info))
                    {
                        LogHandler.WriteServiceLog(GetUserId(), "Id:" + info.Id + ",Operator:" + info.Operator, "成功", "创建", "财务数据");
                    }
                    else
                    {
                        string ErrorCol = errors.Error;
                        LogHandler.WriteServiceLog(GetUserId(), "Id:" + info.Id + ",Operator:" + info.Operator + "," + ErrorCol, "失败", "创建", "财务数据");
                        return Json(JsonHandler.CreateMessage(0, Resource.InsertFail + ErrorCol), JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 删除财务数据
        [HttpPost]
        //[SupportFilter]
        public JsonResult Delete(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                if (m_BLL.m_Rep.Delete(Convert.ToInt32(id)) > 0)
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + id, "成功", "删除", "财务数据");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + id + "," + ErrorCol, "失败", "删除", "财务数据");
                    return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}