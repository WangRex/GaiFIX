using System;
using System.ComponentModel.DataAnnotations;
using Apps.Models;
namespace Apps.Models.Flow
{
    public partial class Flow_FormContentStepCheckState
    {
        [Key]
        public int Id { get; set; }
        public string KEY_Id { get; set; }
        [MaxWordsExpression(50)]
        [Display(Name = "StepCheckId")]
        public string StepCheckId { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "UserId")]
        public string UserId { get; set; }

        [Display(Name = "CheckFlag")]
        public string CheckFlag { get; set; }

        [MaxWordsExpression(2000)]
        [Display(Name = "Reamrk")]
        public string Reamrk { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "TheSeal")]
        public string TheSeal { get; set; }

        [Display(Name = "CreateTime")]
        public string CreateTime { get; set; }

    }
}
