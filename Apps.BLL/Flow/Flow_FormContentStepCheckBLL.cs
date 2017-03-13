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
    public partial class Flow_FormContentStepCheckBLL 
    {
        public Flow_FormContentStepCheckRepository m_Rep;
        public Flow_FormContentStepCheckBLL()
        {
            m_Rep = new Flow_FormContentStepCheckRepository();
        }
       

        public void ResetCheckStateByFormCententId(string stepCheckId, string contentId, int checkState, int checkFlag)
        {
            //base.DbContext.P_Flow_ResetCheckStepState(stepCheckId, contentId, checkState, checkFlag);
        }

        public IQueryable<Flow_FormContentStepCheck> GetListByFormId(string formId, string contentId)
        {
            IQueryable<Flow_FormContentStepCheck> list = from a in m_Rep.DbContext.Flow_FormContentStepCheck
                                                         join b in m_Rep.DbContext.Flow_Step
                                                         on a.StepId equals b.Id.ToString()
                                                         where b.FormId == formId & a.ContentId == contentId
                                                         select a;
            return list;
        }

    }
}
