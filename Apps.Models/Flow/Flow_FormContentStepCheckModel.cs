using System;
using System.ComponentModel.DataAnnotations;
using Apps.Models;
namespace Apps.Models.Flow
{
    public partial class Flow_FormContentStepCheck
    {
        [Key]
        public int Id { get; set; }
        public string KEY_Id { get; set; }
        [MaxWordsExpression(50)]
        [Display(Name = "所属公文")]
        public string ContentId { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "StepId")]
        public string StepId { get; set; }

        [Display(Name = "0不通过1通过2审核中")]
        public string State { get; set; }

        [Display(Name = "true此步骤审核完成")]
        public string StateFlag { get; set; }

        [Display(Name = "创建时间")]
        public string CreateTime { get; set; }

        [Display(Name = "IsEnd")]
        public string IsEnd { get; set; }


        //等于 1 时候为自选
        public string IsCustom { get; set; }
        public string FormId { get; set; }
    }
}
