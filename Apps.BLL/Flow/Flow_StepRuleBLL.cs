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
using Apps.Locale;

namespace Apps.BLL.Flow
{
    public partial class Flow_StepRuleBLL
    {
        public Flow_StepRuleRepository m_Rep;
        public Flow_StepRuleBLL()
        {
            m_Rep = new Flow_StepRuleRepository();
        }

        public Flow_FormAttrRepository attrRep = new Flow_FormAttrRepository();


        public Flow_StepRepository stepRep = new Flow_StepRepository();
        public List<Flow_StepRule> GetList(string stepId)
        {

            IQueryable<Flow_StepRule> queryData = null;

                queryData = m_Rep.FindList(a => a.StepId==stepId);


            return queryData.ToList();
        }


        public bool ValidAttr(string attrId,string Result)
        {
            Flow_FormAttr stepModel = attrRep.Find(Convert.ToInt32(attrId));
            if (stepModel.AttrType == "数字")
            {
                try
                {
                    Convert.ToInt32(Result);
                    return true;
                }
                catch {
                    return false;
                }
            }
            if (stepModel.AttrType == "日期")
            {
                try
                {
                    Convert.ToDateTime(Result);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

    }
}
