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
using Apps.Web.Core;
using Apps.Models.Enum;
using Apps.Locale;
using Apps.BLL.Sys;
using System;

namespace Apps.Web.Areas.Flow.Controllers
{
    public class ExamineController : BaseController
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
            
            return View();
        }
        [HttpPost]
        public JsonResult GetListByUserId(GridPager pager, string queryStr)
        {
            List<Flow_FormContent> list = formContentBLL.GeExamineListByUserId(ref pager, queryStr, GetUserId()).ToList();
            var json = new
            {
                total = pager.totalRows,
                rows = (from r in list
                        select new Flow_FormContent()
                        {

                            Id = r.Id,
                            Title = r.Title,
                            UserId = r.UserId,
                            UserName = r.UserName,
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
       
       

        #region 详细
        //[SupportFilter(ActionName = "Edit")]
        public ActionResult Details(string id)
        {
            
            Flow_Form flowFormModel = formBLL.m_Rep.Find(Convert.ToInt32(id));
            //获取现有的步骤
            flowFormModel.stepList = new List<Flow_Step>();
            flowFormModel.stepList = stepBLL.m_Rep.FindPageList(ref setNoPagerAscById, a => a.FormId == flowFormModel.Id.ToString()).ToList();
            for (int i = 0; i < flowFormModel.stepList.Count; i++)//获取步骤下面的步骤规则
            {
                flowFormModel.stepList[i].stepRuleList = new List<Flow_StepRule>();
                flowFormModel.stepList[i].stepRuleList = stepRuleBLL.m_Rep.FindList(a => a.StepId == flowFormModel.stepList[i].Id.ToString()).ToList();
            }

            return View(flowFormModel);
        }
        #endregion

        //[SupportFilter]
        public ActionResult Edit(string formId, string id)
        {
            //获得当前步骤ID
            string currentStepId = formContentBLL.GetCurrentStepCheckId(formId, id);
            
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
            ViewBag.StepCheckMes = formContentBLL.GetCurrentStepCheckMes(ref setNoPagerAscById, formId, id, GetUserId());
            ViewBag.StepCheckId = currentStepId;
            ViewBag.IsCustomMember = false;

            if(!string.IsNullOrEmpty(currentStepId))
            { 
                List<Flow_FormContentStepCheck> stepCheckModelList = stepCheckBLL.GetListByFormId(formId,id).ToList();
                int j = 0;//下一个步骤
                for (int i = 0; i < stepCheckModelList.Count(); i++)
                {
                    if (currentStepId == stepCheckModelList[i].Id.ToString())
                    {
                        j = i;
                    }
                }
                //获得下个步骤
                if (j + 1 < stepCheckModelList.Count())
                { 
                    //查询第二步是否是自选
                    Flow_FormContentStepCheck stepModel = stepCheckModelList[j + 1];
                    if (stepModel.IsCustom == "true")
                    {
                        ViewBag.IsCustomMember = true;
                    }

                }
            }
            Flow_FormContent model = formContentBLL.m_Rep.Find(Convert.ToInt32(id));
            return View(model);
        }
        //批阅
        [HttpPost]
        //[SupportFilter]
        public JsonResult Edit(string Remark, string TheSeal, string FormId, int Flag, string ContentId,string UserList)
        {
            string stepCheckId = formContentBLL.GetCurrentStepCheckId(FormId, ContentId);
            if (stepCheckId == "")
            {
                return Json(JsonHandler.CreateMessage(0, Resource.EditFail));
            }
            Flow_FormContentStepCheckState stepCheckStateModel = stepCheckStateBLL.m_Rep.Find(a => a.StepCheckId == stepCheckId);
            if (stepCheckStateModel.UserId != GetUserId())
            {
                return Json(JsonHandler.CreateMessage(0, "越权操作！"));
            }
            stepCheckStateModel.Reamrk = Remark;
            stepCheckStateModel.TheSeal = TheSeal;
            stepCheckStateModel.CheckFlag = Flag.ToString();
            if (stepCheckStateBLL.m_Rep.Update (stepCheckStateModel))
            {
                //获取当前步骤
                Flow_FormContentStepCheck stepCheckModel = stepCheckBLL.m_Rep.Find(Convert.ToInt32(stepCheckStateModel.StepCheckId));
                //获得当前的步骤模板
                Flow_Step currentStepModel = stepBLL.m_Rep.Find(Convert.ToInt32(stepCheckModel.StepId));
                //驳回直接终止审核
                if(Flag==(int)FlowStateEnum.Reject)
                {
                    stepCheckModel.State = Flag.ToString();
                    stepCheckModel.StateFlag = "false";
                    stepCheckBLL.m_Rep.Update(stepCheckModel);
                    //重置所有步骤的状态
                    stepCheckBLL.ResetCheckStateByFormCententId(stepCheckModel.Id.ToString(), ContentId, (int)FlowStateEnum.Progress, (int)FlowStateEnum.Progress);
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + stepCheckStateModel.Id + ",StepCheckId" + stepCheckStateModel.Reamrk, "成功", "修改", "Flow_FormContentStepCheckState");
                    return Json(JsonHandler.CreateMessage(1, Resource.CheckSucceed));
                }
                else if (currentStepModel.IsAllCheck == "true")
                {
                    //启用会签
                    //获得同步骤的同批审核人
                    List<Flow_FormContentStepCheckState> stepCheckStateList = stepCheckStateBLL.m_Rep.FindPageList(ref setNoPagerAscById, a => a.StepCheckId == stepCheckStateModel.StepCheckId).ToList();
                    //查看自己是否是最后一个审核人
                    bool complete = stepCheckStateList.Where(a => a.CheckFlag == FlowStateEnum.Progress.ToString()).Count() == 1;
                    if (complete)
                    {
                        stepCheckModel.State = Flag.ToString();
                        stepCheckModel.StateFlag = "true";
                        stepCheckBLL.m_Rep.Update(stepCheckModel);
                    }
                    else {
                        //让审核人继续执行这个步骤直到完成
                        LogHandler.WriteServiceLog(GetUserId(), "Id" + stepCheckStateModel.Id + ",StepCheckId" + stepCheckStateModel.Reamrk, "成功", "修改", "Flow_FormContentStepCheckState");
                        return Json(JsonHandler.CreateMessage(1, Resource.CheckSucceed));
                    }
                }
                else
                {
                    //不是会签，任何一个审批都通过
                    stepCheckModel.State = Flag.ToString();
                    stepCheckModel.StateFlag = "true";
                    stepCheckBLL.m_Rep.Update(stepCheckModel);
                }
                //查看下一步是否为自创建
                if (stepCheckModel.IsEnd != "true" && !string.IsNullOrEmpty(UserList))
                {
                    List<Flow_FormContentStepCheck> stepCheckList = stepCheckBLL.GetListByFormId(FormId, ContentId).ToList();
                    int j = 0;
                    for (int i = stepCheckList.Count() - 1; i >= 0; i--)
                    {
                        if (stepCheckId == stepCheckList[i].Id.ToString())
                        {
                            j = i;
                        }
                    }
                    //查看是否还有下一步步骤
                    if(j-1<=stepCheckList.Count())
                    {
                        //查有第二步骤，查看是否是自选
                        Flow_Step stepModel = stepBLL.m_Rep.Find(Convert.ToInt32(stepCheckList[j + 1].StepId));
                        if (stepModel.FlowRule==FlowRuleEnum.Customer.ToString())
                        {
                            foreach (string userId in UserList.Split(','))
                            {
                                //批量建立步骤审核人表
                                CreateCheckState(stepCheckList[j + 1].Id.ToString(), userId);
                            }
                        }
                        else {
                            //批量建立审核人员表
                            foreach (string userId in GetStepCheckMemberList(stepCheckList[j + 1].StepId))
                            {
                                //批量建立步骤审核人表
                                CreateCheckState(stepCheckList[j + 1].Id.ToString(), userId);
                            }
                        }

                    }
                    
                }


                LogHandler.WriteServiceLog(GetUserId(), "Id" + stepCheckStateModel.Id + ",StepCheckId" + stepCheckStateModel.Reamrk, "成功", "修改", "Flow_FormContentStepCheckState");
                return Json(JsonHandler.CreateMessage(1, Resource.CheckSucceed));
            }
            else
            {
                string ErrorCol = errors.Error;
                LogHandler.WriteServiceLog(GetUserId(), "Id" + stepCheckStateModel.Id + ",StepCheckId" + stepCheckStateModel.Reamrk + "," + ErrorCol, "失败", "修改", "Flow_FormContentStepCheckState");
                return Json(JsonHandler.CreateMessage(0, Resource.CheckFail + ErrorCol));
            }

        }
        //创建审核人
        private void CreateCheckState(string stepCheckId, string userId)
        {
            Flow_FormContentStepCheckState stepCheckModelState = new Flow_FormContentStepCheckState();
            stepCheckModelState.StepCheckId = stepCheckId;
            stepCheckModelState.UserId = userId;
            stepCheckModelState.CheckFlag = "2";
            stepCheckModelState.Reamrk = "";
            stepCheckModelState.TheSeal = "";
            stepCheckModelState.CreateTime = ResultHelper.NowTime.ToString("yyyy-MM-dd");
            stepCheckStateBLL.m_Rep.Create(stepCheckModelState);
        }
        //获得审核人列表
        private List<string> GetStepCheckMemberList(string stepId)
        {
            List<string> userModelList = new List<string>();
            Flow_Step model = stepBLL.m_Rep.Find(Convert.ToInt32(stepId));
            if (model.FlowRule == FlowRuleEnum.Lead.ToString())
            {
                SysUser userModel = userBLL.m_Rep.Find(Convert.ToInt32(GetUserId()));
                string[] array = userModel.Lead.Split(',');//获得领导，可能有多个领导
                foreach (string str in array)
                {
                    userModelList.Add(str);
                }
            }
            else if (model.FlowRule == FlowRuleEnum.Position.ToString())
            {
                string[] array = model.Execution.Split(',');//获得领导，可能有多个领导
                foreach (string str in array)
                {
                    List<SysUser> userList = userBLL.m_Rep.FindList(a => a.PosId == str).ToList();
                    foreach (SysUser userModel in userList)
                    {
                        userModelList.Add(userModel.Id.ToString());
                    }
                }
            }
            else if (model.FlowRule == FlowRuleEnum.Department.ToString())
            {
                GridPager pager = new GridPager()
                {
                    rows = 10000,
                    page = 1,
                    sort = "Id",
                    order = "desc"
                };
                string[] array = model.Execution.Split(',');//获得领导，可能有多个领导
                foreach (string str in array)
                {
                    List<SysUser> userList = userBLL.m_Rep.FindPageList(ref pager, a => a.DepId == str).ToList();
                    foreach (SysUser userModel in userList)
                    {
                        userModelList.Add(userModel.Id.ToString());
                    }
                }
            }
            else if (model.FlowRule == FlowRuleEnum.Person.ToString())
            {
                string[] array = model.Execution.Split(',');//获得领导，可能有多个领导
                foreach (string str in array)
                {
                    userModelList.Add(str);
                }
            }
          
            return userModelList;
        }

        #region 获得表单
        //根据设定公文，生成表单及控制条件
        private string ExceHtmlJs(string id)
        {
            //定义一个sb为生成HTML表单
            StringBuilder sbHtml = new StringBuilder();
            StringBuilder sbJS = new StringBuilder();
            sbJS.Append("<script type='text/javascript'>function CheckForm(){");
            Flow_Form model = formBLL.m_Rep.Find(Convert.ToInt32(id));

            #region 判断流程是否有字段,有就生成HTML表单
            sbHtml.Append(JuageExc(model.AttrA, "A", ref sbJS));
            sbHtml.Append(JuageExc(model.AttrB, "B", ref sbJS));
            sbHtml.Append(JuageExc(model.AttrC, "C", ref sbJS));
            sbHtml.Append(JuageExc(model.AttrD, "D", ref sbJS));
            sbHtml.Append(JuageExc(model.AttrE, "E", ref sbJS));
            sbHtml.Append(JuageExc(model.AttrF, "F", ref sbJS));
            sbHtml.Append(JuageExc(model.AttrG, "G", ref sbJS));
            sbHtml.Append(JuageExc(model.AttrH, "H", ref sbJS));
            sbHtml.Append(JuageExc(model.AttrI, "I", ref sbJS));
            sbHtml.Append(JuageExc(model.AttrJ, "J", ref sbJS));
            sbHtml.Append(JuageExc(model.AttrK, "K", ref sbJS));
            sbHtml.Append(JuageExc(model.AttrL, "L", ref sbJS));
            sbHtml.Append(JuageExc(model.AttrM, "M", ref sbJS));
            sbHtml.Append(JuageExc(model.AttrN, "N", ref sbJS));
            sbHtml.Append(JuageExc(model.AttrO, "O", ref sbJS));
            sbHtml.Append(JuageExc(model.AttrP, "P", ref sbJS));
            sbHtml.Append(JuageExc(model.AttrQ, "Q", ref sbJS));
            sbHtml.Append(JuageExc(model.AttrR, "R", ref sbJS));
            sbHtml.Append(JuageExc(model.AttrS, "S", ref sbJS));
            sbHtml.Append(JuageExc(model.AttrT, "T", ref sbJS));
            sbHtml.Append(JuageExc(model.AttrU, "U", ref sbJS));
            sbHtml.Append(JuageExc(model.AttrV, "V", ref sbJS));
            sbHtml.Append(JuageExc(model.AttrW, "W", ref sbJS));
            sbHtml.Append(JuageExc(model.AttrX, "X", ref sbJS));
            sbHtml.Append(JuageExc(model.AttrY, "Y", ref sbJS));
            sbHtml.Append(JuageExc(model.AttrZ, "Z", ref sbJS));
            #endregion
            sbJS.Append("return true}</script>");
            return sbJS.ToString()+sbHtml.ToString();
        }
        //对比
        private bool JudgeVal(string attrId, string rVal, string cVal, string lVal)
        {
            string attrType = formAttrBLL.m_Rep.Find(Convert.ToInt32(attrId)).AttrType;
            return new FlowHelper().Judge(attrType, rVal, cVal, lVal);
        }
        private string JuageExc(string attr, string no, ref StringBuilder sbJS)
        {

            if (!string.IsNullOrEmpty(attr))
            {
                return GetHtml(attr, no, ref sbJS);

            }
            return "";
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
        #endregion

    }
}
