/**
* 命名空间: Apps.BLL.LianTong
*
* 功 能： N/A
* 类 名： LianTong_ProjectContractsApproveHisBLL
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
* V0.01 2017-2-25 8:53:23 王仁禧 初版
*
* Copyright (c) 2017 Lir Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：大连安琪科技有限公司 　　　　　　　　　　　　　　       │
*└──────────────────────────────────┘
*/

using Apps.DAL.LianTong;

namespace Apps.BLL.LianTong
{
    /// <summary>
    /// 合同审批记录
    /// </summary>
    public partial class LianTong_ProjectContractsApproveHisBLL
    {
        public LianTong_ProjectContractsApproveHisRepository m_Rep;

        public LianTong_ProjectContractsApproveHisBLL() {
            m_Rep = new LianTong_ProjectContractsApproveHisRepository();
        }
    }
}
