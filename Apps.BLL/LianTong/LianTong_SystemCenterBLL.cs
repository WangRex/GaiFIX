using Apps.Common;
using Apps.DAL.LianTong;
using Apps.Models.LianTong;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Apps.BLL.LianTong
{
    public partial class LianTong_SystemCenterBLL
    {
        public LianTong_SystemCenterRepository m_Rep;
        public LianTong_SystemCenterBLL()
        {
            m_Rep = new LianTong_SystemCenterRepository();
        }

        #region 首页合同金额数
        public Dictionary<string, long> ContratcMoneyCnt()
        {
            long contractCost = 0;
            long equipmentCost = 0;
            long projectCost = 0;
            long Profit = 0;
            List<LianTong_SystemCenterModel> reportList = m_Rep.FindList().ToList();
            Dictionary<string, long> RefList = new Dictionary<string, long>();
            foreach (LianTong_SystemCenterModel item in reportList)
            {
                contractCost += Convert.ToInt64(item.contractCost);
                equipmentCost += Convert.ToInt64(item.equipmentCost);
                projectCost += Convert.ToInt64(item.projectCost);
            }
            Profit = contractCost - equipmentCost - projectCost;
            RefList.Add("contractCost", contractCost);
            RefList.Add("equipmentCost", equipmentCost);
            RefList.Add("projectCost", projectCost);
            RefList.Add("Profit", Profit);
            return RefList;
        }
        #endregion

        #region 获取利润
        public string getProfit(LianTong_SystemCenterModel _LianTong_SystemCenterModel)
        {
            var profit = ((_LianTong_SystemCenterModel.contractCost == null ? 0 : _LianTong_SystemCenterModel.contractCost) - (_LianTong_SystemCenterModel.equipmentCost == null ? 0 : _LianTong_SystemCenterModel.equipmentCost) - (_LianTong_SystemCenterModel.projectCost == null ? 0 : _LianTong_SystemCenterModel.projectCost)).ToString();
            return profit;
        }
        #endregion

        #region 获取利润率
        public string getProfitRate(LianTong_SystemCenterModel _LianTong_SystemCenterModel)
        {
            if(_LianTong_SystemCenterModel.contractCost == null)
            {
                return "0.00%";
            }
            var profit = ((_LianTong_SystemCenterModel.contractCost == null ? 0 : _LianTong_SystemCenterModel.contractCost) - (_LianTong_SystemCenterModel.equipmentCost == null ? 0 : _LianTong_SystemCenterModel.equipmentCost) - (_LianTong_SystemCenterModel.projectCost == null ? 0 : _LianTong_SystemCenterModel.projectCost));
            if(profit == null)
            {
                return "0.00%";
            }
            return String.Format("{0:F}", (profit / _LianTong_SystemCenterModel.contractCost) * 100) + '%';
        }
        #endregion

    }
}
