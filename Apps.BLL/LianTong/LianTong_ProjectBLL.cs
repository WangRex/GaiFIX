using Apps.Common;
using Apps.DAL.LianTong;
using Apps.DAL.Spl;
using Apps.Models;
using Apps.Models.LianTong;
using Apps.Models.Spl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.BLL.LianTong
{
    public partial class LianTong_ProjectBLL
    {
        public LianTong_ProjectRepository m_Rep;
        public LianTong_ProjectBLL()
        {
            m_Rep = new LianTong_ProjectRepository();
        }

        public string GetProjectChildNum(string NumStart)
        {
            int ChildNum = 1;
            //获取实体列表
            IQueryable<LianTong_ProjectModel> _LianTong_Contracts = m_Rep.FindList().Where(cm => cm.projectNum.StartsWith(NumStart));
            foreach (LianTong_ProjectModel _model in _LianTong_Contracts)
            {
                int newNum = Convert.ToInt32(_model.projectNum.Replace(NumStart, ""));
                if (newNum >= ChildNum)
                {
                    ChildNum = newNum + 1;
                }
            }
            return ChildNum.ToString().PadLeft(3, '0');
        }

    }
}
