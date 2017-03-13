using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using Apps.Models;
using Apps.Common;
using System.Transactions;
using Apps.Models.Flow;
using Apps.BLL.Core;
using Apps.Locale;
using Apps.DAL.Flow;

namespace Apps.BLL.Flow
{
    public partial class Flow_FormAttrBLL
    {


        public Flow_TypeRepository typeRep = new Flow_TypeRepository();
        public Flow_FormAttrRepository m_Rep;
        public Flow_FormAttrBLL()
        {
            m_Rep = new Flow_FormAttrRepository();
        }

        public List<Flow_FormAttr> GetList(ref GridPager pager, string typeId)
        {
            IQueryable<Flow_FormAttr> queryData = null;
            if (!string.IsNullOrWhiteSpace(typeId) && typeId != "0")
            {

                queryData = m_Rep.FindList(a => a.Name.Contains(typeId) || a.TypeId == typeId);
            }
            else
            {
                queryData = m_Rep.FindList();
            }
            pager.totalRows = queryData.Count();
            queryData = LinqHelper.SortingAndPaging(queryData, pager.sort, pager.order, pager.page, pager.rows);
            return queryData.ToList();
        }
    }
}
