using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Apps.Common;

using Apps.Models.Sys;
using Microsoft.Practices.Unity;
using Apps.BLL.Flow;
using Apps.Models.Flow;
using System.Text;
using System;
using Apps.Web.Core;
using Apps.Models.Enum;
using Apps.Locale;
using Apps.BLL.Flow;
using Apps.BLL.Sys;

namespace Apps.Web.Areas.Flow.Controllers
{
       
        
    public class DrafController : BaseController
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

        public ActionResult Index()
        {
            List<Flow_Type> list = m_BLL.m_Rep.FindPageList(ref setNoPagerAscBySort, "").ToList();
            foreach (var v in list)
            {
                v.formList = new List<Flow_Form>();
                List<Flow_Form> formList = formBLL.m_Rep.FindList(a => a.TypeId == v.Id.ToString()).ToList();
                v.formList = formList;
            }
            ViewBag.DrafList = list;
            return View();

            
        }
       

        [HttpPost]
        //[SupportFilter]
        public JsonResult Create(Flow_FormContent model)
        {
            //当前的Form模版
            Flow_Form formModel = formBLL.m_Rep.Find(Convert.ToInt32(model.FormId));
            //初始化部分数据
            model.CreateTime = ResultHelper.NowTime.ToString("yyyy-MM-dd");
            model.UserId = GetUserId();
            model.Title = formModel.Name;
            model.TimeOut = DateTime.Now.AddDays(30).ToString("yyyy-MM-dd");
            if (model != null)
            {

                if (formContentBLL.m_Rep.Create(model))
                {
                    //创建成功后把步骤取出
                    List<Flow_Step> stepModelList = stepBLL.m_Rep.FindPageList(ref setNoPagerAscBySort, a => a.FormId == model.FormId).ToList();
                    //查询步骤
                    int listCount = stepModelList.Count();
                    string IsEnd = "false";
                    for (int i = 0; i < listCount; i++)
                    {
                        string nextStep  ="";
                        Flow_Step stepModel = stepModelList[i];
                        List<Flow_StepRule> stepRuleModelList = stepRuleBLL.m_Rep.FindList(a => a.StepId == stepModel.Id.ToString()).ToList();
                        //获取规则判断流转方向
                        foreach (Flow_StepRule stepRuleModel in stepRuleModelList)
                        {
                           string val =new FlowHelper().GetFormAttrVal(stepRuleModel.AttrId, formModel, model);
                            //有满足流程结束的条件
                           if (!JudgeVal(stepRuleModel.AttrId, val, stepRuleModel.Operator, stepRuleModel.Result))
                           {
                               if (stepRuleModel.NextStep != "0")
                               {
                                   //获得跳转的步骤
                                   nextStep = stepRuleModel.NextStep;
                                   //跳到跳转后的下一步
                                   for (int j = 0; j < listCount; j++)
                                   {
                                       if (stepModelList[j].Id.ToString() == nextStep)
                                       {
                                           i = j;//跳到分支后的下一步
                                       }
                                   }
                               }
                               else
                               {
                                   //nextStep=0流程结束
                                   IsEnd = "true";
                               }
                           }
                          
                        }
                        
                        #region 插入步骤
                        //插入步骤审核表
                        Flow_FormContentStepCheck stepCheckModel = new Flow_FormContentStepCheck();
                        stepCheckModel.ContentId = model.Id.ToString();
                        stepCheckModel.StepId = stepModel.Id.ToString();
                        stepCheckModel.State = FlowStateEnum.Progress.ToString();
                        stepCheckModel.StateFlag = "false";//true此步骤审核完成
                        stepCheckModel.CreateTime = ResultHelper.NowTime.ToString("yyyy-MM-dd");
                        if (nextStep != "" && nextStep != "0")
                        {
                            stepCheckModel.IsEnd = (i == listCount) ? "true" : IsEnd;//是否流程的最后一步
                        }
                        else
                        {
                            stepCheckModel.IsEnd = (i == listCount-1) ? "true" : IsEnd;//是否流程的最后一步
                        }
                        
                        stepCheckModel.IsCustom = stepModel.FlowRule == FlowRuleEnum.Customer.ToString() ? "true" : "false";
                        if (stepCheckBLL.m_Rep.Create(stepCheckModel))//新建步骤成功
                        {
                            InsertChecker(model, i, stepModel, stepCheckModel);
                        }
                        #endregion
                        #region 插入分支步骤
                        if (nextStep != "" && nextStep != "0")
                        {
                            //不是最后一个审核人
                            if (listCount < 1 || i != listCount - 1)
                            {
                                IsEnd = "false";
                            }
                            else
                            {
                                IsEnd = "true";
                            }

                            stepCheckModel = new Flow_FormContentStepCheck();
                            stepCheckModel.ContentId = model.Id.ToString();
                            stepCheckModel.StepId = stepModel.Id.ToString();
                            stepCheckModel.State = FlowStateEnum.Progress.ToString();
                            stepCheckModel.StateFlag = "false";//true此步骤审核完成
                            stepCheckModel.CreateTime = ResultHelper.NowTime.ToString("yyyy-MM-dd");
                            stepCheckModel.IsEnd = IsEnd;//是否流程的最后一步
                            

                            stepCheckModel.IsCustom = stepModel.FlowRule == FlowRuleEnum.Customer.ToString() ? "true" : "false";
                            if (stepCheckBLL.m_Rep.Create(stepCheckModel))//新建步骤成功
                            {
                                InsertChecker(model, i, stepModel, stepCheckModel);
                            }

                        }
                        #endregion

                        if (IsEnd == "true")//如果是最后一步就无需要下面继续了
                        {
                            break;
                        }
                        IsEnd = "true";
                    }



                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",AttrA" + model.AttrA, "成功", "创建", "Flow_FormContent");
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",AttrA" + model.AttrA + "," + ErrorCol, "失败", "创建", "Flow_FormContent");
                    return Json(JsonHandler.CreateMessage(0, Resource.InsertFail + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.InsertFail));
            }
        }

        private void InsertChecker(Flow_FormContent model, int i, Flow_Step stepModel, Flow_FormContentStepCheck stepCheckModel)
        {
            //获得流转规则下的审核人员
            List<string> userIdList = new List<string>();
            if (stepModel.FlowRule == FlowRuleEnum.Customer.ToString())
            {
                string[] arrUserList = model.CustomMember.Split(',');
                foreach (string s in arrUserList)
                {
                    userIdList.Add(s);
                }
            }
            else
            {
                userIdList = GetStepCheckMemberList(stepModel.Id, model.Id);
            }

                foreach (string userId in userIdList)
                {
                    //批量建立步骤审核人表
                    Flow_FormContentStepCheckState stepCheckModelState = new Flow_FormContentStepCheckState();
                    stepCheckModelState.StepCheckId = stepCheckModel.Id.ToString();
                    stepCheckModelState.UserId = userId;
                    stepCheckModelState.CheckFlag = "2";
                    stepCheckModelState.Reamrk = "";
                    stepCheckModelState.TheSeal = "";
                    stepCheckModelState.CreateTime = ResultHelper.NowTime.ToString("yyyy-MM-dd");
                    stepCheckStateBLL.m_Rep.Create(stepCheckModelState);
                }
        }

        public List<string> GetStepCheckMemberList(int stepId,int formContentId)
        {
            List<string> userModelList = new List<string>();
            Flow_Step model = stepBLL.m_Rep.Find(stepId);
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
            else if (model.FlowRule == FlowRuleEnum.Customer.ToString())
            {
                string users  = formContentBLL.m_Rep.Find(Convert.ToInt32(formContentId)).CustomMember;
                string[] array = users.Split(',');//获得领导，可能有多个领导
                foreach (string str in array)
                {
                    userModelList.Add(str);
                }
            }
            return userModelList;
        }
        //对比
        private bool JudgeVal(string attrId, string rVal, string cVal, string lVal)
        {
            string attrType = formAttrBLL.m_Rep.Find(Convert.ToInt32(attrId)).AttrType;
            return new FlowHelper().Judge(attrType, rVal, cVal, lVal);
        }


        //[SupportFilter]
        public ActionResult Create(string id)
        {
            
            Flow_Form formModel = formBLL.m_Rep.Find(Convert.ToInt32(id));
            //是否已经设置布局
            if (!string.IsNullOrEmpty(formModel.HtmlForm))
            {
                ViewBag.Html = formModel.HtmlForm;
            }
            else
            {
                ViewBag.Html = ExceHtmlJs(id);
            }
            Flow_FormContent model = new Flow_FormContent();
            model.FormId = id;
            //创建成功取出步骤
            List<Flow_Step> stepModelList = stepBLL.m_Rep.FindPageList(ref setNoPagerAscById, a => a.FormId == model.FormId).ToList();
            Flow_Step stepModel = stepBLL.m_Rep.Find(Convert.ToInt32(stepModelList[0].Id));
            if (stepModel.FlowRule == FlowRuleEnum.Customer.ToString())
            {
                ViewBag.Checker = null;
            }
            else
            {
                List<string> users = GetStepCheckMemberList(stepModel.Id,Convert.ToInt32(id));
                ViewBag.Checker = users;
            }
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
            //获得对象的类型，model
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
                    sbHtml.Append(JuageExc(o.ToString(), str, ref sbJS));
                }
            }
            #endregion
            sbJS.Append("return true}</script>");
            return sbHtml.ToString()+sbJS.ToString();
        }

        private string JuageExc(string attr, string no,ref StringBuilder sbJS)
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
            sb.AppendFormat("<tr><th>{0} :</th>", attrModel.Title);
            //获取指定类型的HTML表单
            sb.AppendFormat("<td>{0}</td></tr>", new FlowHelper().GetInput(attrModel.AttrType, attrModel.Name, no));
            sbJS.Append(attrModel.CheckJS);
            return sb.ToString();
        }
    }
}
