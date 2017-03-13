using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Apps.Common;

using Apps.Models.Sys;
using Microsoft.Practices.Unity;
using Apps.BLL.Flow;
using Apps.Models.Flow;
using System.Text;
using Apps.Models;
using Apps.Core.OnlineStat;
using Apps.Web.Core;
using System;
using System.Reflection;
using Apps.Locale;

namespace Apps.Web.Areas.Flow.Controllers
{
    public class FormController : BaseController
    {
        
        public Flow_FormBLL m_BLL = new Flow_FormBLL();
        
        public Flow_FormAttrBLL attrBLL = new Flow_FormAttrBLL();
        
        public Flow_TypeBLL typeBLL = new Flow_TypeBLL();
        
        public Flow_StepBLL stepBLL = new Flow_StepBLL();
        
        public Flow_StepRuleBLL stepRuleBLL = new Flow_StepRuleBLL();
        
        public Flow_FormAttrBLL formAttrBLL = new Flow_FormAttrBLL();
        ValidationErrors errors = new ValidationErrors();

        //[SupportFilter]
        public ActionResult Index()
        {
            
            return View();
        }

        [HttpPost]
        public JsonResult GetFormAttrList(GridPager pager, string queryStr)
        {
            List<Flow_FormAttr> list = attrBLL.GetList(ref pager, queryStr);
            var json = new
            {
                total = pager.totalRows,
                rows = (from r in list
                        select new Flow_FormAttr()
                        {
                            Id = r.Id,
                            Title = r.Title,
                            Name = r.Name,
                            AttrType = r.AttrType,
                            CheckJS = r.CheckJS,
                            TypeId = r.TypeId,
                            CreateTime = r.CreateTime

                        }).ToArray()

            };

            return Json(json);
        }

        [HttpPost]
        public JsonResult GetList(GridPager pager, string queryStr)
        {
            List<Flow_Form> list = m_BLL.m_Rep.FindPageList(ref pager, queryStr).ToList();
            var json = new
            {
                total = pager.totalRows,
                rows = (from r in list
                        select new Flow_Form()
                        {

                            Id = r.Id,
                            Name = r.Name,
                            Remark = r.Remark,
                            UsingDep = r.UsingDep,
                            TypeName = r.TypeName,
                            State = r.State,
                            CreateTime = r.CreateTime

                        }).ToArray()

            };

            return Json(json);
        }

        #region 创建
        //[SupportFilter]
        public ActionResult Create()
        {
            
            List<Flow_Type> list = typeBLL.m_Rep.FindPageList(ref setNoPagerAscBySort, "").ToList();
            ViewBag.FlowType = new SelectList(list, "Id", "Name");
            
            return View();
        }

        [HttpPost]
        //[SupportFilter]
        public JsonResult Create(Flow_Form model)
        {
            model.CreateTime = ResultHelper.NowTime.ToString("yyyy-MM-dd");
            if (model != null)
            {

                if (m_BLL.m_Rep.Create(model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name, "成功", "创建", "Flow_Form");
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name + "," + ErrorCol, "失败", "创建", "Flow_Form");
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
        //[SupportFilter]
        public ActionResult Edit(string id)
        {
            Flow_Form model = m_BLL.m_Rep.Find(Convert.ToInt32(id));
            //得到已经选择的字段
            StringBuilder sb = new StringBuilder();
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
                    //查找model类的Class对象的"str"属性的值
                    sb.Append(GetAttr(o.ToString(), str));
                }
            }
            ViewBag.AttrList = sb.ToString();
            List<Flow_Type> list = typeBLL.m_Rep.FindPageList(ref setNoPagerAscBySort, "").ToList();
            ViewBag.FlowType = new SelectList(list, "Id", "Name",model.TypeId);
            ViewBag.FlowTypeName = new SelectList(list, "Id", "Name");
            return View(model);
        }
        //获取已经添加的字段
        private string GetAttr(string id,string str)
        {
           
                Flow_FormAttr model = attrBLL.m_Rep.Find(Convert.ToInt32(id));
                return "<tr id='tr" + str + "'><td style='text-align:right'>" + model.Title + "：</td>" +
                "<td>" + getExample(model.AttrType) + "<input id='" + str + "' name='" + str + "' type='hidden' value='" + model.Id + "' /></td>" +
                "<td><a href=\"javascript:deleteCurrentTR('tr" + str + "');\">[删除]</a></td></tr>";
          
        }

       private string getExample(string v)
        {
            switch (v)
            {
                case "文本": return "<input type='text' />"; 
                case "多行文本": return "<textarea></textarea>"; 
                case "数字": return "<input type='text' />"; 
                case "日期": return "<input type='text' />"; 
                default: return "";
            }
        }
        [HttpPost]
        //[SupportFilter]
        public JsonResult Edit(Flow_Form model)
        {
            if (model != null)
            {

                if (m_BLL.m_Rep.Update(model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name, "成功", "修改", "Flow_Form");
                    return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name + "," + ErrorCol, "失败", "修改", "Flow_Form");
                    return Json(JsonHandler.CreateMessage(0, Resource.EditFail + ":"+ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.EditFail));
            }
        }
        #endregion

        #region 设计表单布局
        //[SupportFilter(ActionName = "Edit")]
        public ActionResult FormLayout(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View();
            }
            Flow_Form formModel = m_BLL.m_Rep.Find(Convert.ToInt32(id));
            //是否已经设置布局
            if (!string.IsNullOrEmpty(formModel.HtmlForm))
            {
                ViewBag.Html = formModel.HtmlForm;
            }
            else
            {
                ViewBag.Html = ExceHtmlJs(id);
            }
            ViewBag.FormId = id;
            
            
            
            return View();
        }
        private string ExceHtmlJs(string id)
        {
            //定义一个sb为生成HTML表单
            StringBuilder sbHtml = new StringBuilder();
            StringBuilder sbJS = new StringBuilder();
            sbJS.Append("<script type='text/javascript'>function CheckForm(){");
            Flow_Form model = m_BLL.m_Rep.Find(Convert.ToInt32(id));
            #region 判断流程是否有字段,有就生成HTML表单
            Type formType = model.GetType();
            //查找名称为"A-Z"的属性
            string[] arrStr = { "AttrA", "AttrB", "AttrC", "AttrD", "AttrE", "AttrF", "AttrG", "AttrH", "AttrI", "AttrJ", "AttrK"
                                  , "AttrL", "AttrM", "AttrN", "AttrO", "AttrP", "AttrQ", "AttrR", "AttrS", "AttrT", "AttrU"
                                  , "AttrV", "AttrW", "AttrX", "AttrY", "AttrZ"};
            sbHtml.AppendFormat("<div class='easyui-draggable' data-option='onDrag:onDrag'><table class='inputtable'><tr><td style='text-align:center'>{0}</td></tr></table></div>", model.Name);
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
            return sbHtml.ToString()+sbJS.ToString();
        }

        private string GetHtml(string id, string no, ref StringBuilder sbJS)
        {
            StringBuilder sb = new StringBuilder();
            Flow_FormAttr attrModel = formAttrBLL.m_Rep.Find(Convert.ToInt32(id));
            sb.AppendFormat("<div class='easyui-draggable' data-option='onDrag:onDrag'><table class='inputtable'><tr><td style='vertical-align:middle' class='inputtitle'>{0}</td>", attrModel.Title);
            //获取指定类型的HTML表单
            sb.AppendFormat("<td class='inputcontent'>{0}</td></tr></table></div>", new FlowHelper().GetInput(attrModel.AttrType, attrModel.Name, no));
            sbJS.Append(attrModel.CheckJS);
            return sb.ToString();
        }

        [HttpPost]
        //[SupportFilter(ActionName = "Edit")]
        [ValidateInput(false)]
        public JsonResult SaveLayout(string  html,string formId)
        {

            Flow_Form model = m_BLL.m_Rep.Find(Convert.ToInt32(formId));
            model.HtmlForm = html;
            if (m_BLL.m_Rep.Update(model))
            {
                LogHandler.WriteServiceLog(GetUserId(), "Id:" + model.Id + ",Name:" + model.Name, "成功", "修改", "表单布局");
                return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
            }
            else
            {
                string ErrorCol = errors.Error;
                LogHandler.WriteServiceLog(GetUserId(), "Id:" + model.Id + ",Name:" + model.Name + "," + ErrorCol, "失败", "修改", "表单布局");
                return Json(JsonHandler.CreateMessage(0, Resource.EditFail + ":"+ErrorCol));
            }
            
        }
        #endregion


        #region 设计步骤
        //[SupportFilter(ActionName = "Edit")]
        public ActionResult EditStep(string id)
        {
            
            Flow_Form flowFormModel = m_BLL.m_Rep.Find(Convert.ToInt32(id));
             List<Flow_Step> stepList = stepBLL.m_Rep.FindPageList(ref setNoPagerAscById, a => a.FormId == flowFormModel.Id.ToString()).ToList();//获得全部步骤
            foreach (var r in stepList)//获取步骤下面的步骤规则
            {
                r.stepRuleList = GetStepRuleListByStepId(r.Id.ToString());
            }
            flowFormModel.stepList = stepList;//获取表单关联的步骤
            ViewBag.Form = flowFormModel;
            Flow_Step model = new Flow_Step();
            model.FormId = flowFormModel.Id.ToString();
            model.IsEditAttr = "true";
            return View(model);
        }

     

        [HttpPost]
        //[SupportFilter(ActionName = "Edit")]
        public JsonResult EditStep(Flow_Step model)
        {
            if (model != null)
            {

                if (stepBLL.m_Rep.Create(model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name, "成功", "创建", "Flow_Step");
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed, model.Id.ToString()));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name + "," + ErrorCol, "失败", "创建", "Flow_Step");
                    return Json(JsonHandler.CreateMessage(0, Resource.InsertFail + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.InsertFail));
            }
        }
        [HttpPost]
        //[SupportFilter(ActionName = "Edit")]
        public JsonResult DeleteStep(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                if (stepBLL.m_Rep.Delete(Convert.ToInt32(id))> 0)
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + id, "成功", "删除", "Flow_Step");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + id + "," + ErrorCol, "失败", "删除", "Flow_Step");
                    return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail));
            }


        }



        #endregion 
        //获取已经添加的字段
        private List<Flow_FormAttr> GetAttrList(Flow_Form model)
        {
            List<Flow_FormAttr> list = new List<Flow_FormAttr>();
            Flow_FormAttr attrModel = new Flow_FormAttr();
            #region 处理字段
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
                    //查找model类的Class对象的"str"属性的值
                    attrModel = attrBLL.m_Rep.Find(Convert.ToInt32(o)); 
                    list.Add(attrModel);
                }
            }
            #endregion
            return list;
        }

        //获取步骤下面的规则
        private List<Flow_StepRule> GetStepRuleListByStepId(string stepId)
        {
            List<Flow_StepRule> list = stepRuleBLL.GetList(stepId);
            return list;
        }
        #region 详细
       //[SupportFilter(ActionName = "Edit")]
        public ActionResult Details(string id)
        {
            
            Flow_Form flowFormModel = m_BLL.m_Rep.Find(Convert.ToInt32(id));
            //获取现有的步骤
            GridPager pager = new GridPager()
            {
                rows = 1000,
                page = 1,
                sort = "Id",
                order = "asc"
            };
            flowFormModel.stepList = new List<Flow_Step>();
            flowFormModel.stepList = stepBLL.m_Rep.FindPageList(ref pager, a => a.FormId == flowFormModel.Id.ToString()).ToList();
            for (int i = 0; i < flowFormModel.stepList.Count; i++)//获取步骤下面的步骤规则
            {
                flowFormModel.stepList[i].stepRuleList = new List<Flow_StepRule>();
                flowFormModel.stepList[i].stepRuleList = GetStepRuleListByStepId(flowFormModel.stepList[i].Id.ToString());
            }

            return View(flowFormModel);
        }

        #endregion

        #region 删除
        [HttpPost]
        //[SupportFilter]
        public JsonResult Delete(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                if (m_BLL.m_Rep.Delete(Convert.ToInt32(id))>0)
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + id, "成功", "删除", "Flow_Form");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + id + "," + ErrorCol, "失败", "删除", "Flow_Form");
                    return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail));
            }


        }
        #endregion

        #region 设计分支
        //[SupportFilter(ActionName = "Edit")]
        public ActionResult StepList(string id)
        {
            ViewBag.FormId = id;
            return View();
        }
        [HttpPost]
        //[SupportFilter(ActionName = "Edit")]
        public JsonResult GetStepList(GridPager pager, string id)
        {
            List<Flow_Step> stepList = stepBLL.m_Rep.FindPageList(ref pager, a => a.FormId == id).ToList();
            int i = 1;
            var json = new
            {
                total = pager.totalRows,
                rows = (from r in stepList
                        select new Flow_Step()
                        {
                            StepNo = "第 "+(i++)+" 步",
                            Id = r.Id,
                            Name = r.Name,
                            Remark = r.Remark,
                            Sort = r.Sort,
                            FormId = r.FormId,
                            FlowRule = r.FlowRule,
                            Action = "<a href='javascript:SetRule(\"" + r.Id + "\")'>分支(" + GetStepRuleListByStepId(r.Id.ToString()).Count() + ")</a></a>"
                        }).ToArray()

            };
            return Json(json);
        }
        //[SupportFilter(ActionName = "Edit")]
        public ActionResult StepRuleList(string stepId,string formId)
        {
            //获取现有的步骤
            GridPager pager = new GridPager()
            {
                rows = 1000,
                page = 1,
                sort = "Id",
                order = "desc"
            };
            
            Flow_Form flowFormModel = m_BLL.m_Rep.Find(Convert.ToInt32(formId));
            List<Flow_FormAttr>  attrList = new List<Flow_FormAttr>();//获取表单关联的字段
            attrList = GetAttrList(flowFormModel);
            List<Flow_Step> stepList = stepBLL.m_Rep.FindPageList(ref pager, a => a.FormId == formId).ToList();

            ViewBag.StepId = stepId;
            ViewBag.AttrList = attrList;
            ViewBag.StepList = stepList;
            return View();
        }
        [HttpPost]
        //[SupportFilter(ActionName = "Edit")]
        public JsonResult GetStepRuleList(string stepId)
        {
            List<Flow_StepRule> stepList = stepRuleBLL.GetList(stepId);
            int i =1;
            var json = new
            {
                rows = (from r in stepList
                        select new Flow_StepRule()
                        {
                            Mes="分支"+(i++),
                            Id = r.Id,
                            StepId = r.StepId,
                            AttrId = r.AttrId,
                            AttrName = r.AttrName,
                            Operator = r.Operator,
                            Result = r.Result,
                            NextStep = r.NextStep,
                            NextStepName = r.NextStepName
                        }).ToArray()

            };

            return Json(json);
        }


        [HttpPost]
        //[SupportFilter(ActionName = "Edit")]
        public JsonResult CreateStepEvent(Flow_StepRule model)
        {
            if (model != null)
            {

                if (stepRuleBLL.m_Rep.Create(model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",StepId" + model.Id, "成功", "创建", "Flow_StepRule");
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed, model.Id.ToString()));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",StepId" + model.Id + "," + ErrorCol, "失败", "创建", "Flow_StepRule");
                    return Json(JsonHandler.CreateMessage(0, Resource.InsertFail + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.InsertFail));
            }
        }


        [HttpPost]
        //[SupportFilter(ActionName = "Edit")]
        public JsonResult DeleteStepRule(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                if (stepRuleBLL.m_Rep.Delete(Convert.ToInt32(id))>0)
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + id, "成功", "删除", "Flow_StepRule");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + id + "," + ErrorCol, "失败", "删除", "Flow_StepRule");
                    return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail));
            }


        }
        #endregion 
    }
}
