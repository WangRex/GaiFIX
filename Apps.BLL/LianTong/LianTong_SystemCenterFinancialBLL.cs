using Apps.DAL.LianTong;

namespace Apps.BLL.LianTong
{
    public partial class LianTong_SystemCenterFinancialBLL
    {
        public LianTong_SystemCenterFinancialRepository m_Rep;
        public LianTong_SystemCenterFinancialBLL()
        {
            m_Rep = new LianTong_SystemCenterFinancialRepository();
        }
    }
}
