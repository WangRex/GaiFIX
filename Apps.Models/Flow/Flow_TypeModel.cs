using System;
using System.ComponentModel.DataAnnotations;
using Apps.Models;
using System.Collections.Generic;
namespace Apps.Models.Flow
{
    public partial class Flow_Type
    {
        [Key]
        public int Id { get; set; }
        public string KEY_Id { get; set; }
        [MaxWordsExpression(50)]
        [Display(Name = "类别")]
        public string Name { get; set; }

        [MaxWordsExpression(500)]
        [Display(Name = "说明")]
        public string Remark { get; set; }

        [Display(Name = "创建时间")]
        public string CreateTime { get; set; }

        [Display(Name = "排序")]
        public string Sort { get; set; }

        public List<Flow_Form> formList { get; set; }

    }
}
