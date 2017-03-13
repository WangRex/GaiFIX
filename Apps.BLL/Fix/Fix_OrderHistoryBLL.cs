/**
* 命名空间: Apps.BLL.Fix
*
* 功 能： N/A
* 类 名： Fix_OrderHistoryBLL
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
* V0.01 2017-3-12 10:39:51 王仁禧 初版
*
* Copyright (c) 2017 Lir Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：大连安琪科技有限公司 　　　　　　　　　　　　　　       │
*└──────────────────────────────────┘
*/
using Apps.DAL.Fix;

namespace Apps.BLL.Fix
{
    public partial class Fix_OrderHistoryBLL
    {
        public Fix_OrderHistoryRepository m_Rep;
        public Fix_OrderHistoryBLL()
        {
            m_Rep = new Fix_OrderHistoryRepository();
        }
    }
}
