using System.ComponentModel.DataAnnotations;

namespace Apps.Models.FIX
{
    public class FIX_AssessmentModel : FIX_BaseModels
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 评价内容
        /// </summary>
        [Display(Name = "评价内容")]
        public string Content { get; set; }

        /// <summary>
        /// 问题处理
        /// </summary>
        [Display(Name = "问题处理")]
        public int QuestionStar { get; set; }

        /// <summary>
        /// 服务态度
        /// </summary>
        [Display(Name = "服务态度")]
        public int AttitudeStar { get; set; }

        /// <summary>
        /// 综合评价
        /// </summary>
        [Display(Name = "综合评价")]
        public int ComprehensiveStar { get; set; }

        /// <summary>
        /// 关联外线员ID
        /// </summary>
        [Display(Name = "关联外线员ID")]
        public int OTM_ID { get; set; }

        /// <summary>
        /// 关联订单ID
        /// </summary>
        [Display(Name = "关联订单ID")]
        public int Order_ID { get; set; }

        /// <summary>
        /// 关联用户手机号
        /// </summary>
        [Display(Name = "关联用户手机号")]
        public string Phone { get; set; }
    }
}