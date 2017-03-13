using System.ComponentModel.DataAnnotations;

namespace Apps.Models.FIX
{
    public class FIX_MatchingModel : FIX_BaseModels
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 匹配地址
        /// </summary>
        [Display(Name = "匹配地址")]
        public string Address { get; set; }

        /// <summary>
        /// 状态
        /// 未绑定/已绑定
        /// </summary>
        [Display(Name = "状态")]
        public string Status { get; set; }

        /// <summary>
        /// 外线员
        /// </summary>
        [Display(Name = "外线员")]
        public string OTM { get; set; }

        /// <summary>
        /// 外线员
        /// </summary>
        [Display(Name = "外线员")]
        public string OTM_Name { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        [Display(Name = "部门")]
        public string department { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [Display(Name = "部门名称")]
        public string departmentName { get; set; }
    }
}