using Apps.BLL.Core;
using Apps.Common;
using Apps.DAL.WC;
using Apps.Models;
using Apps.Models.WC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.BLL.WC
{
    public partial class WC_UserBLL
    {
        public WC_UserRepository m_Rep;
        public WC_UserBLL()
        {
            m_Rep = new WC_UserRepository();
        }

        public List<WC_User> GetList(ref GridPager pager, string queryStr)
        {
            IQueryable<WC_User> queryData = m_Rep.FindPageList(ref pager, queryStr);
            return queryData.ToList();
        }

    }
}
