using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using Apps.Models;
using Apps.Common;
using System.Transactions;
using Apps.Models.Flow;
using Apps.DAL.Flow;

using Apps.BLL.Core;
using Apps.Models.Enum;
using Apps.Locale;

namespace Apps.BLL.Flow
{
    public partial class Flow_FormContentBLL 
    {
        public Flow_FormContentRepository m_Rep;
        public Flow_FormContentBLL()
        {
            m_Rep = new Flow_FormContentRepository();
        }
        public Flow_StepRepository stepBLL = new Flow_StepRepository();


        public Flow_StepRuleRepository stepRuleBLL = new Flow_StepRuleRepository();

        public Flow_FormContentStepCheckBLL stepCheckBLL = new Flow_FormContentStepCheckBLL();

        public Flow_FormContentStepCheckStateRepository stepCheckStateBLL = new Flow_FormContentStepCheckStateRepository();

        public List<Flow_FormContent> GetListByUserId(ref GridPager pager, string queryStr, string userId)
        {
            IQueryable<Flow_FormContent> queryData = null;
            if (!string.IsNullOrWhiteSpace(queryStr))
            {
                queryData = m_Rep.FindList(a => a.Title.Contains(queryStr) && a.UserId==userId);
            }
            else
            {
                queryData = m_Rep.FindList(a=>a.UserId == userId);
            }
            pager.totalRows = queryData.Count();
            queryData = LinqHelper.SortingAndPaging(queryData, pager.sort, pager.order, pager.page, pager.rows);
            return queryData.ToList();
        }

    
        public string GetCurrentFormState(Flow_FormContent model)
        {
            if (Convert.ToDateTime(model.TimeOut) <ResultHelper.NowTime)
            {
                return FlowStateEnum.Closed.ToString();
            }
            List<Flow_FormContentStepCheck> stepCheckModelList = stepCheckBLL.GetListByFormId(model.FormId, model.KEY_Id).ToList();


            var passList = from r in stepCheckModelList where r.State == FlowStateEnum.Pass.ToString() select r;
            if (passList.Count() == stepCheckModelList.Count())
            {
                return FlowStateEnum.Pass.ToString();
            }
            var rejectList = from r in stepCheckModelList where r.State == FlowStateEnum.Reject.ToString() select r;
            if (rejectList.Count() > 0)
            {
                return FlowStateEnum.Reject.ToString();
            }
            return FlowStateEnum.Progress.ToString();
        }
        public string GetCurrentFormStep(Flow_FormContent model)
        {
            string stepName = GetCurrentFormStepModel(model).Name;
            if(!string.IsNullOrEmpty(stepName))
            {
                return stepName;
            }
            return "结束";
        }

        public Flow_Step GetCurrentFormStepModel(Flow_FormContent model)
        {
            List<Flow_FormContentStepCheck> stepCheckModelList = stepCheckBLL.GetListByFormId(model.FormId, model.Id.ToString()).ToList();
            for (int i =0;i< stepCheckModelList.Count();i++)
            {
                if (stepCheckModelList[i].State == FlowStateEnum.Progress.ToString())
                {
                    return stepBLL.Find(Convert.ToInt32(stepCheckModelList[i].StepId));
                }
            }
            return new Flow_Step();
        }
       
      
        //获取环节所有信息
        public string GetCurrentStepCheckMes(ref GridPager pager, string formId, string contentId,string currentUserId)
        {
            string stepCheckMes = "";
            List<Flow_FormContentStepCheck> stepCheckModelList = stepCheckBLL.GetListByFormId(formId, contentId).ToList();
            for (int i = 0; i < stepCheckModelList.Count; i++)
            {
                string strStepCheckId = stepCheckModelList[i].Id.ToString();
                List<Flow_FormContentStepCheckState> stepCheckStateList = stepCheckStateBLL.FindPageList(ref pager, a => a.StepCheckId == strStepCheckId).ToList();
                stepCheckMes = stepCheckMes + "<tr><th style='width:150px'>第" + (i + 1) + "步--->审核人：</th><td><table >";
                foreach (Flow_FormContentStepCheckState checkStateModel in stepCheckStateList)
                {
                    stepCheckMes += "<tr class='" + (checkStateModel.UserId == currentUserId ? "color-green" : "") + "'><td>" + checkStateModel.UserId + " </td><th style='width:90px'> 审核意见：</th><td>" + checkStateModel.Reamrk + "</td><th style='width:90px'>审核结果：</th><td>" + (checkStateModel.CheckFlag == FlowStateEnum.Pass.ToString() ? "通过" : checkStateModel.CheckFlag == FlowStateEnum.Reject.ToString() ? "驳回" : "审核中") + "</td></tr>";
                }
                if (stepCheckStateList.Count == 0)
                {
                    stepCheckMes += "<tr><td>【等待上一步审核】</td><th> </th><td></td><th></th><td></td></tr>";
                }
                stepCheckMes += "</table></td></tr>";
            }
            return stepCheckMes;
        }

        public IQueryable<Flow_FormContent> GeExamineListByUserId(ref GridPager pager, string queryStr,string userId)
        {
            IQueryable<Flow_FormContent> queryable = m_Rep.GeExamineListByUserId(userId);
            if (!string.IsNullOrWhiteSpace(queryStr))
            {
                queryable = queryable.Where(a => a.Title.Contains(queryStr));
            }

            pager.totalRows = queryable.Count();
            queryable = LinqHelper.SortingAndPaging(queryable, pager.sort, pager.order, pager.page, pager.rows);
            return queryable;
        }

        public IQueryable<Flow_FormContent> GeExamineList()
        {
            IQueryable<Flow_FormContent> list = m_Rep.GeExamineList();
            return list;
        }

        public List<Flow_FormContent> GeExaminetList(ref GridPager pager, string queryStr)
        {
            IQueryable<Flow_FormContent> queryData = null;
            if (!string.IsNullOrWhiteSpace(queryStr))
            {
                queryData = m_Rep.GeExamineList().Where(a => a.Title.Contains(queryStr));
            }
            else
            {
                queryData = m_Rep.GeExamineList();
            }
            pager.totalRows = queryData.Count();
            queryData = LinqHelper.SortingAndPaging(queryData, pager.sort, pager.order, pager.page, pager.rows);
            return queryData.ToList();
        }

        public string GetCurrentStepCheckId(string formId, string contentId)
        {
            List<Flow_FormContentStepCheck> stepCheckModelList = stepCheckBLL.m_Rep.GetListByFormId(formId, contentId).ToList();
            return new FlowHelper().GetCurrentStepCheckIdByStepCheckModelList(stepCheckModelList);

        }

    }
}
