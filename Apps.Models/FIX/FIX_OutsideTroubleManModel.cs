using System.ComponentModel.DataAnnotations;

namespace Apps.Models.FIX
{
    /// <summary>
    /// 外线员模型
    /// <remarks>
    /// 创建：2017.02.03<br />
    /// </remarks>
    /// </summary>
    public class FIX_OutsideTroubleManModel : FIX_BaseModels
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 外线员姓名
        /// </summary>
        [Display(Name = "外线员姓名")]
        public string Name { get; set; }

        /// <summary>
        /// 外线员头像
        /// </summary>
        [Display(Name = "外线员头像")]
        public string Photo { get; set; }

        /// <summary>
        /// 外线员电话
        /// </summary>
        [Display(Name = "外线员电话")]
        public string Phone { get; set; }

        /// <summary>
        /// 外线员工龄
        /// </summary>
        [Display(Name = "外线员工龄")]
        public string WorkYear { get; set; }

        /// <summary>
        /// 外线员工号
        /// </summary>
        [Display(Name = "外线员工号")]
        public string EmployeeNo { get; set; }

        /// <summary>
        /// 外线员登陆密码
        /// </summary>
        [Display(Name = "外线员登陆密码")]
        public string Password { get; set; }

        /// <summary>
        /// 外线员所属部门
        /// </summary>
        [Display(Name = "外线员所属部门")]
        public string Department { get; set; }

        /// <summary>
        /// 外线员业务内容
        /// </summary>
        [Display(Name = "外线员业务内容")]
        public string Business { get; set; }

        /// <summary>
        /// 外线员负责区域简介
        /// </summary>
        [Display(Name = "外线员负责区域简介")]
        public string ResponsibleAreaBrief { get; set; }

        /// <summary>
        /// 外线员状态
        /// 1: 可工作; 2: 不可工作;
        /// </summary>
        [Display(Name = "外线员状态")]
        public string Status { get; set; }

        /// <summary>
        /// 入职时间
        /// </summary>
        [Display(Name = "入职时间")]
        public string OnboardTime { get; set; }

        /// <summary>
        /// 离职时间
        /// </summary>
        [Display(Name = "离职时间")]
        public string OffboardTime { get; set; }

        /// <summary>
        /// 外线员备注
        /// </summary>
        [Display(Name = "外线员备注")]
        public string Remark { get; set; }

    }
}
