using System;
using System.ComponentModel.DataAnnotations;
using Apps.Models;
using System.Collections.Generic;
namespace Apps.Models.Flow
{
    public partial class Flow_Step
    {
        [Key]
        public int Id { get; set; }
        public string KEY_Id { get; set; }
        [MaxWordsExpression(50)]
        [Display(Name = "步骤名称")]
        //[NotNullExpression]
        public string Name { get; set; }

        [MaxWordsExpression(500)]
        [Display(Name = "步骤说明")]
        public string Remark { get; set; }

        [Display(Name = "排序")]
        public string Sort { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "所属表单")]
        public string FormId { get; set; }

        [Display(Name = "流转规则")]
        public string FlowRule { get; set; }

        [Display(Name = "自选审批人")]
        public string IsCustom { get; set; }

        [Display(Name = "是否全审核")]
        public string IsAllCheck { get; set; }

        [MaxWordsExpression(4000)]
        [Display(Name = "审批人")]
        public string Execution { get; set; }

        [Display(Name = "强制完成")]
        public string CompulsoryOver { get; set; }

        [Display(Name = "编辑附件")]
        public string IsEditAttr { get; set; }

        public  string Action { get; set; }
        public  string StepNo { get; set; }

        public List<Flow_StepRule> stepRuleList { get; set; }
    }
}
