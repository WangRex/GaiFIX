/**
* 命名空间: Apps.BLL.Fix
*
* 功 能： N/A
* 类 名： Fix_AssessmentBLL
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
* V0.01 2017/3/10 15:13:58 王仁禧 初版
*
* Copyright (c) 2017 Lir Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：大连安琪科技有限公司 　　　　　　　　　　　　　　       │
*└──────────────────────────────────┘
*/
using Apps.DAL.Fix;
using Apps.Models.FIX;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Apps.BLL.Fix
{
    public class Fix_AssessmentBLL
    {
        public Fix_AssessmentRepository m_Rep;
        public Fix_AssessmentBLL()
        {
            m_Rep = new Fix_AssessmentRepository();
        }

        public FIX_AssessmentModel getAssessment(int orderId)
        {
            //获取实体列表
            IQueryable<FIX_AssessmentModel> _FIX_AssessmentModels = m_Rep.FindList().Where(a => a.Order_ID == orderId);
            FIX_AssessmentModel _FIX_AssessmentModel = null;
            if(_FIX_AssessmentModels.ToList().Count > 0)
            {
                _FIX_AssessmentModel = _FIX_AssessmentModels.First();
            }
            return _FIX_AssessmentModel;
        }
    }
}
