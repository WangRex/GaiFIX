using Apps.Common;
using Apps.DAL.Spl;
using Apps.Models;
using Apps.Models.Spl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.BLL.Spl
{
    public partial class Spl_ProductBLL
    {
        public Spl_ProductRepository m_Rep;
        public Spl_ProductBLL()
        {
            m_Rep = new Spl_ProductRepository();
        }
        public List<Spl_Product> GetList(ref GridPager pager, string queryStr)
        {

            IQueryable<Spl_Product> queryData = null;
            if (!string.IsNullOrWhiteSpace(queryStr))
            {
                queryData = m_Rep.FindList(
								a=>a.KEY_Id.Contains(queryStr)
								|| a.Name.Contains(queryStr)
								|| a.Code.Contains(queryStr)
								
								|| a.Color.Contains(queryStr)
								
								|| a.CategoryId.Contains(queryStr)
								
								|| a.CreateBy.Contains(queryStr)
								
								);
            }
            else
            {
                queryData = m_Rep.FindList();
            }
        
            //启用通用列头过滤
            if (!string.IsNullOrWhiteSpace(pager.filterRules))
            {
                List<DataFilterModel> dataFilterList = JsonHandler.Deserialize<List<DataFilterModel>>(pager.filterRules).Where(f => !string.IsNullOrWhiteSpace(f.value)).ToList();
                queryData = LinqHelper.DataFilter<Spl_Product>(queryData, dataFilterList);
            }

            pager.totalRows = queryData.Count();
            //排序
            queryData = LinqHelper.SortingAndPaging(queryData, pager.sort, pager.order, pager.page, pager.rows);
            return queryData.ToList();
        }

    }
}
