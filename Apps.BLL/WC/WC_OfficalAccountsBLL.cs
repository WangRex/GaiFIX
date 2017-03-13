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
    public partial class WC_OfficalAccountsBLL
    {
        public WC_OfficalAccountsRepository m_Rep;
        public WC_OfficalAccountsBLL()
        {
            m_Rep = new WC_OfficalAccountsRepository();
        }

        public WC_OfficalAccounts GetCurrentAccount()
        {
            WC_OfficalAccounts entity = m_Rep.Find(p => p.IsDefault == "true");
            if (entity == null)
            {
                return new WC_OfficalAccounts();
            }
            return entity;
        }
        public bool SetDefault(int id)
        {
            WC_OfficalAccounts model = m_Rep.Find(id);
            if (model == null)
            {
                return false;
            }
            else
            {
                //更新所有为不默认0
                //设置当前为默认1
                model.IsDefault = "true";
                return m_Rep.Update(model);
            }
        }

        public List<WC_OfficalAccounts> GetList(ref GridPager pager, string queryStr)
        {
            IQueryable<WC_OfficalAccounts> queryData = m_Rep.FindPageList(ref pager, queryStr);
            return queryData.ToList();
        }
    }
}
