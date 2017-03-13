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
    public partial class Spl_ProductCategoryBLL
    {
        public Spl_ProductCategoryRepository m_Rep;
        public Spl_ProductCategoryBLL()
        {
            m_Rep = new Spl_ProductCategoryRepository();
        }

    }
}
