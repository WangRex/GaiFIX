using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Models.LianTong
{
    /// <summary>
    /// 合同审批历史模型
    /// </summary>
    public class LianTong_ContractApproveHistoryModels
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "合同ID")]
        public int ContractID { get; set; }

        [Display(Name = "审批人")]
        public string ApproveName { get; set; }

        [Display(Name = "审批时间")]
        public DateTime ApproveDate { get; set; }

        [Display(Name = "审批旧状态")]
        public string ApproveOldStatus { get; set; }

        [Display(Name = "审批新状态")]
        public string ApproveNewStatus { get; set; }
    }
}
