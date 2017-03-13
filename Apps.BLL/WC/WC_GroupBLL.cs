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
    public partial class WC_GroupBLL
    {
        public WC_GroupRepository m_Rep;
        public WC_GroupBLL()
        {
            m_Rep = new WC_GroupRepository();
        }

        public List<WC_Group> GetList(ref GridPager pager, string queryStr)
        {
            IQueryable<WC_Group> queryData = m_Rep.FindPageList(ref pager, queryStr);
            return queryData.ToList();
        }
    }
}
