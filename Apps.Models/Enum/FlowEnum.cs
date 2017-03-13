using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Models.Enum
{
    public enum FlowStateEnum
    {
        /// <summary>
        /// 驳回
        /// </summary>
        Reject =0,
        /// <summary>
        /// 通过
        /// </summary>
        Pass = 1,

        /// <summary>
        /// 进行中
        /// </summary>
        Progress =2,

        /// <summary>
        /// 关闭
        /// </summary>
        Closed = 3
    }

    public enum FlowRuleEnum
    { 
        /// <summary>
        /// 上级
        /// </summary>
        Lead =1,
        /// <summary>
        /// 人员
        /// </summary>
        Person = 2,
        /// <summary>
        /// 自选
        /// </summary>
        Customer = 3,
        /// <summary>
        /// 职位
        /// </summary>
        Position = 4,
        /// <summary>
        /// 部门
        /// </summary>
        Department =5,
    }
    public enum FlowFormLevelEnum
    {
        /// <summary>
        /// 普通
        /// </summary>
        Ordinary = 1,
        /// <summary>
        /// 重要
        /// </summary>
        Major = 2,
        /// <summary>
        /// 紧急
        /// </summary>
        Urgent = 3,
        /// <summary>
        /// 未通过
        /// </summary>
        NotPass = 4,
        /// <summary>
        /// 已过期
        /// </summary>
        Overdue = 5


    }

    public enum FlowLianTongContracts
    {
        /// <summary>
        /// 送审
        /// </summary>
        送审 = 1,
        /// <summary>
        /// 补全
        /// </summary>
        补全 = 2,
        /// <summary>
        /// 审订
        /// </summary>
        审订 = 3,
        /// <summary>
        /// 开票
        /// </summary>
        开票 = 4,
        /// <summary>
        /// 回款
        /// </summary>
        回款 = 5,
        /// <summary>
        /// 完结
        /// </summary>
        完结 = 6
    }

}
