using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Apps.Common;

using Apps.Models.Sys;
using Microsoft.Practices.Unity;
using Apps.BLL.Flow;
using Apps.Models.Flow;
using System.Text;
using Apps.BLL.Flow;
using System;
using Apps.Web.Core;
using Apps.Models.Enum;
using Apps.BLL.Sys;

namespace Apps.Web.Areas.Flow.Controllers
{
    public class ApplyController : BaseController
    {
        
        public SysUserBLL userBLL = new SysUserBLL();
        
        public Flow_TypeBLL m_BLL = new Flow_TypeBLL();
        
        public Flow_FormBLL formBLL = new Flow_FormBLL();
        
        public Flow_FormAttrBLL formAttrBLL = new Flow_FormAttrBLL();
        
        public Flow_FormContentBLL formContentBLL = new Flow_FormContentBLL();
        
        public Flow_StepBLL stepBLL = new Flow_StepBLL();
        
        public Flow_StepRuleBLL stepRuleBLL = new Flow_StepRuleBLL();
        
        public Flow_FormContentStepCheckBLL stepCheckBLL = new Flow_FormContentStepCheckBLL();
        
        public Flow_FormContentStepCheckStateBLL stepCheckStateBLL = new Flow_FormContentStepCheckStateBLL();

        ValidationErrors errors = new ValidationErrors();

        //[SupportFilter]
        public ActionResult Index()
        {
            
            
            List<Flow_FormContent> list = formContentBLL.GeExamineListByUserId(ref setNoPagerAscById,"",GetUserId()).ToList();
            foreach (var model in list)
            {
                List<Flow_FormContentStepCheck> stepCheckModelList = stepCheckBLL.GetListByFormId(model.FormId, model.Id.ToString()).ToList();
                model.CurrentState = formContentBLL.GetCurrentFormState(model);
            }
            FlowStateCount stateModel = new FlowStateCount();
            stateModel.requestCount = list.Count();
            stateModel.passCount = list.Where(a => a.CurrentState == FlowStateEnum.Pass.ToString()).Count();
            stateModel.rejectCount = list.Where(a => a.CurrentState == FlowStateEnum.Reject.ToString()).Count();
            stateModel.processCount = list.Where(a => a.CurrentState == FlowStateEnum.Progress.ToString()).Count();
            stateModel.closedCount = list.Where(a => Convert.ToDateTime(a.TimeOut) < DateTime.Now).Count();
            return View(stateModel);
        }
        [HttpPost]
        public JsonResult GetListByUserId(GridPager pager, string queryStr)
        {
            List<Flow_FormContent> list = formContentBLL.GetListByUserId(ref pager, queryStr, GetUserId());
            var json = new
            {
                total = pager.totalRows,
                rows = (from r in list
                        select new Flow_FormContent()
                        {

                            Id = r.Id,
                            Title = r.Title,
                            UserId = r.UserId,
                            FormId = r.FormId,
                            FormLevel = r.FormLevel,
                            CreateTime = r.CreateTime,
                            TimeOut = r.TimeOut,
                            CurrentStep = formContentBLL.GetCurrentFormStep(r),
                            CurrentState = formContentBLL.GetCurrentFormState(r)
                            
                        }).ToArray()

            };
            return Json(json);
        }
        public string GetCurrentStep(Flow_FormContent model)
        {
            string str = "结束";
            List<Flow_FormContentStepCheck> stepCheckModelList = stepCheckBLL.GetListByFormId(model.FormId,model.Id.ToString()).ToList();
            for (int i = stepCheckModelList.Count()-1;i>=0;i--)
            {
                if (stepCheckModelList[i].State == "2")
                {
                    str = stepBLL.m_Rep.Find(Convert.ToInt32(stepCheckModelList[i].StepId)).Name;
                }
            }
            return str;
        }
       

        #region 详细
        //[SupportFilter(ActionName = "Details")]
        public ActionResult Details(string id)
        {
            
            Flow_Form flowFormModel = formBLL.m_Rep.Find(Convert.ToInt32(id));
            //获取现有的步骤
            GridPager pager = new GridPager()
            {
                rows = 1000,
                page = 1,
                sort = "Id",
                order = "asc"
            };
            flowFormModel.stepList = new List<Flow_Step>();
            flowFormModel.stepList = stepBLL.m_Rep.FindPageList(ref pager, (a => a.FormId == flowFormModel.Id.ToString())).ToList();
            for (int i = 0; i < flowFormModel.stepList.Count; i++)//获取步骤下面的步骤规则
            {
                flowFormModel.stepList[i].stepRuleList = new List<Flow_StepRule>();
                flowFormModel.stepList[i].stepRuleList = GetStepRuleListByStepId(flowFormModel.stepList[i].Id.ToString());
            }

            return View(flowFormModel);
        }
        //获取步骤下面的规则
        private List<Flow_StepRule> GetStepRuleListByStepId(string stepId)
        {
            List<Flow_StepRule> list = stepRuleBLL.GetList(stepId);
            return list;
        }
        #endregion
        

        //[SupportFilter(ActionName = "Details")]
        public ActionResult Edit(string formId,string id)
        {

            
            Flow_Form formModel = formBLL.m_Rep.Find(Convert.ToInt32(formId));
            //是否已经设置布局
            if (!string.IsNullOrEmpty(formModel.HtmlForm))
            {
                ViewBag.Html = formModel.HtmlForm;
            }
            else
            {
                ViewBag.Html = ExceHtmlJs(formId);
            }
            ViewBag.StepCheckMes = formContentBLL.GetCurrentStepCheckMes(ref setNoPagerAscById, formId, id,GetUserId());
            Flow_FormContent model = formContentBLL.m_Rep.Find(Convert.ToInt32(id));
            return View(model);
        }
        
             //根据设定公文，生成表单及控制条件
        private string ExceHtmlJs(string id)
        {
            //定义一个sb为生成HTML表单
            StringBuilder sbHtml = new StringBuilder();
            StringBuilder sbJS = new StringBuilder();
            sbJS.Append("<script type='text/javascript'>function CheckForm(){");
            Flow_Form model = formBLL.m_Rep.Find(Convert.ToInt32(id));
            #region 判断流程是否有字段,有就生成HTML表单
            Type formType = model.GetType();
            //查找名称为"A-Z"的属性
            string[] arrStr = { "AttrA", "AttrB", "AttrC", "AttrD", "AttrE", "AttrF", "AttrG", "AttrH", "AttrI", "AttrJ", "AttrK"
                                  , "AttrL", "AttrM", "AttrN", "AttrO", "AttrP", "AttrQ", "AttrR", "AttrS", "AttrT", "AttrU"
                                  , "AttrV", "AttrW", "AttrX", "AttrY", "AttrZ"};
            foreach (string str in arrStr)
            {
                object o = formType.GetProperty(str).GetValue(model, null);
                if (o != null)
                {
                    //查找model类的Class对象的"str"属性的值
                    sbHtml.Append(GetHtml(o.ToString(), str, ref sbJS));
                }
            }
            
     
            #endregion
            sbJS.Append("return true}</script>");
            ViewBag.HtmlJS = sbJS.ToString();
            return sbHtml.ToString();
        }
     
        //对比
        private bool JudgeVal(string attrId, string rVal, string cVal, string lVal)
        {
            string attrType = formAttrBLL.m_Rep.Find(Convert.ToInt32(attrId)).AttrType;
            return new FlowHelper().Judge(attrType, rVal, cVal, lVal);
        }
       




        //获取指定名称的HTML表单
        private string GetHtml(string id, string no, ref StringBuilder sbJS)
        {
            StringBuilder sb = new StringBuilder();
            Flow_FormAttr attrModel = formAttrBLL.m_Rep.Find(Convert.ToInt32(id));
            sb.AppendFormat("<tr><td style='width:100px; text-align:right;'>{0} :</td>", attrModel.Title);
            //获取指定类型的HTML表单
            sb.AppendFormat("<td>{0}</td></tr>", new FlowHelper().GetInput(attrModel.AttrType, attrModel.Name, no));
            sbJS.Append(attrModel.CheckJS);
            return sb.ToString();
        }

        
    }
}
