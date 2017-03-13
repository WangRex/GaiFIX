using System;
using System.ComponentModel.DataAnnotations;
using Apps.Models;
namespace Apps.Models.Sys
{
    public partial class SysPosition
    {
        [Key]
        public int Id { get; set; }
        public string KEY_Id { get; set; }
        [MaxWordsExpression(50)]
        [Display(Name = "职位名称")]
        public string Name { get; set; }

        [MaxWordsExpression(500)]
        [Display(Name = "职位说明")]
        public string Remark { get; set; }

        [Display(Name = "排序")]
        public string Sort { get; set; }

        [Display(Name = "创建时间")]
        public string CreateTime { get; set; }

        [Display(Name = "状态")]
        public string Enable { get; set; }

        [Display(Name = "职位允许人数")]
        public string MemberCount { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "所在部门")]
        public string DepId { get; set; }
        public  string DepName { get; set; }

        public  string Flag { get; set; }
    }
}
