/**
* 命名空间: Apps.BLL.Fix
*
* 功 能： N/A
* 类 名： Fix_OrderBLL
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
* V0.01 2017-3-8 20:21:42 王仁禧 初版
*
* Copyright (c) 2017 Lir Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：大连安琪科技有限公司 　　　　　　　　　　　　　　       │
*└──────────────────────────────────┘
*/
using Apps.DAL.Fix;
using Apps.Models.FIX;
using System.Collections.Generic;
using System.Linq;

namespace Apps.BLL.Fix
{
    public partial class Fix_OrderBLL
    {
        public Fix_OrderRepository m_Rep;
        public Fix_OrderBLL()
        {
            m_Rep = new Fix_OrderRepository();
        }
        public int getOTMServiceNum(int OTM_ID)
        {
            //获取实体列表
            IQueryable<FIX_OrderModel> _Orders = m_Rep.FindList().Where(om => om.Id == OTM_ID && om.Status == "已完成");
            return _Orders.Count();
        }
        public List<FIX_OrderModel> getOrdersByOTM(int OTM_ID, string status)
        {
            var _Orders = m_Rep.FindList().Where(om => om.Id == OTM_ID && om.Status == status).ToList();
            return _Orders;
        }
        public List<FIX_OrderModel> getWaitingAssessmentOrders(string Phone, string Status)
        {
            //获取实体列表
            IQueryable<FIX_OrderModel> _Orders = m_Rep.FindList().Where(om => om.Phone == Phone && om.Status.Equals(Status));
            return _Orders.ToList();
        }
    }
}
