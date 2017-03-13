using System;
using System.ComponentModel.DataAnnotations;
using Apps.Models;
namespace Apps.Models.Flow
{
    public partial class Flow_StepRule
    {
        [Key]
        public int Id { get; set; }
        public string KEY_Id { get; set; }
        [Display(Name = "StepId")]
        public string StepId { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "AttrId")]
        public string AttrId { get; set; }
        public  string AttrName { get; set; }
        [MaxWordsExpression(10)]
        [Display(Name = "Operator")]
        public string Operator { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "Result")]
        public string Result { get; set; }


         [MaxWordsExpression(50)]
        [Display(Name = "NextStep")]
        public string NextStep { get; set; }
         public  string NextStepName { get; set; }
         public  string Mes { get; set; }
         public  string Action { get; set; }
    }
}
