using System;
using System.ComponentModel.DataAnnotations;

namespace Apps.Models.LianTong
{
    /// <summary>
    /// 财务模型
    /// </summary>
    public class LianTong_SystemCenterFinancialModel
        {
            [Key]
            public int Id { get; set; }

            [Display(Name = "日期")]
            public string Date { get; set; }

            [Display(Name = "类型")]
            /// <summary>
            /// 类型（下拉列表：采购/报销）
            /// </summary>
            public string Type { get; set; }

            [Display(Name = "状态")]
            /// <summary>
            /// 状态（下拉列表：申请/已报销）
            /// </summary>
            public string Status { get; set; }

            [Display(Name = "经办/报销人")]
            public string Operator { get; set; }

            [Display(Name = "金额")]
            public Double? Money { get; set; }

            [Display(Name = "来源")]
            public string Source { get; set; }

            [Display(Name = "用途描述")]
            public string Description { get; set; }

            [Display(Name = "发票金额")]
            public Double? Invoice { get; set; }

            [Display(Name = "备注")]
            public string Remark { get; set; }
        }
}
