using System;
using System.Collections.Generic;
using System.Linq;
using Apps.Common;
using Microsoft.Practices.Unity;
using Apps.BLL.MIS;
using Apps.Models;
using Apps.BLL.Core;
using Apps.Models.MIS;
using Apps.DAL.MIS;

namespace Apps.BLL.MIS
{
    public partial class MIS_WebIM_RecentContactBLL
    {
        public MIS_WebIM_RecentContactRepository m_Rep;
        public MIS_WebIM_RecentContactBLL()
        {
            m_Rep = new MIS_WebIM_RecentContactRepository();
        }

        /// <summary>
        /// 返回用户的最近联系人信息
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public List<MIS_WebIM_RecentContact> GetList(ref GridPager pager, string userId)
        {
            IQueryable<MIS_WebIM_RecentContact> queryData = null;
            queryData = m_Rep.FindList(a =>a.UserId==userId).OrderByDescending(a => a.Id);

            pager.totalRows = queryData.Count();
            if (pager.totalRows > 0)
            {
                if (pager.page <= 1)
                {
                    queryData = queryData.Take(pager.rows);
                }
                else
                {
                    queryData = queryData.Skip((pager.page - 1) * pager.rows).Take(pager.rows);
                }
            }

            return queryData.ToList();
        }

    }
}
