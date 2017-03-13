using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

using System.Web.Mvc;

namespace Apps.Models.Sys
{
    public partial class UserView
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string TrueName { get; set; }
        public string Flag { get; set; }

    }
    public partial class SysUser
    {
        [Key]
        
        public int Id { get; set; }
        public string KEY_Id { get; set; }

        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [StringLength(50,MinimumLength=5)]
        //[System.Web.Mvc.Compare("ComparePassword", ErrorMessage = "两次密码不一致")]
        [Display(Name = "密码")]
        public string Password { get; set; }
        //[System.Web.Mvc.Compare("Password", ErrorMessage = "两次密码不一致")]
        [Display(Name = "确认密码")]
        public string ComparePassword { get; set; }
        //[NotNullExpression]
        [Display(Name = "真实名称")]
        public string TrueName { get; set; }
        [Display(Name = "身份证")]
        public string Card { get; set; }
        [Display(Name = "手机号码")]
        //[NotNullExpression]
        public string MobileNumber { get; set; }
        [Display(Name = "电话号码")]
        public string PhoneNumber { get; set; }
        [Display(Name = "QQ")]
        public string QQ { get; set; }
        [Display(Name = "Email")]
        public string EmailAddress { get; set; }
        [Display(Name = "其他联系方式")]
        public string OtherContact { get; set; }
        [Display(Name = "省份")]
        public string Province { get; set; }
        [Display(Name = "城市")]
        public string City { get; set; }
        [Display(Name = "地区")]
        public string Village { get; set; }
        [Display(Name = "详细地址")]
        public string Address { get; set; }
        [Display(Name = "状态")]
        public string State { get; set; }
        [DateExpression]//如果填写判断是否是日期
        [Display(Name = "创建时间")]
        public string CreateTime { get; set; }
        [Display(Name = "创建人")]
        public string CreatePerson { get; set; }
        [Display(Name = "性别")]
        public string Sex { get; set; }
        [DateExpression]//如果填写判断是否是日期
        [Display(Name = "生日")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public string Birthday { get; set; }
        [DateExpression]//如果填写判断是否是日期
        [Display(Name = "加入时间")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public string JoinDate { get; set; }
        [Display(Name = "婚姻状况")]
        public string Marital { get; set; }
        [Display(Name = "党派")]
        public string Political { get; set; }
        [Display(Name = "籍贯")]
        public string Nationality { get; set; }
        [Display(Name = "所在地")]
        public string Native { get; set; }
        [Display(Name = "学校")]
        public string School { get; set; }
        [Display(Name = "专业")]
        public string Professional { get; set; }
        [Display(Name = "学历")]
        public string Degree { get; set; }
        //[NotNullExpression]
        [Display(Name = "部门")]
        public string DepId { get; set; }
        public  string DepName { get; set; }
        //[NotNullExpression]
        [Display(Name = "职位")]
        public string PosId { get; set; }
        [Display(Name = "备注")]
        public string Expertise { get; set; }
        [Display(Name = "是否在职")]
        public  string JobState { get; set; }
        [Display(Name = "照片")]
        public string Photo { get; set; }
        [Display(Name = "附件")]
        public string Attach { get; set; }

        public string RoleId { get; set; }//拥有的用户

        [Display(Name = "角色")]
        public  string RoleName { get; set; }//拥有的用户

        public  string Flag { get; set; }//用户分配角色
        public  string PosName { get; set; }


        [MaxWordsExpression(50)]
        [Display(Name = "上级领导")]
        public string Lead { get; set; }
        public string LeadName { get; set; }
        [Display(Name = "是否可以自选领导")]
        public  string IsSelLead { get; set; }

        [Display(Name = "日否启动日程汇报是否启用  启用后 他的上司领导将可以看到他的 工作日程汇报.")]
        public  string IsReportCalendar { get; set; }

        [Display(Name = "开启 小秘书")]
        public  string IsSecretary { get; set; }
        [Display(Name = "城市")]
        public string CityName { get; set; }
        [Display(Name = "省份")]
        public string ProvinceName { get; set; }
        [Display(Name = "地区")]
        public string VillageName { get; set; }

        /*--------------------------外线员信息-----------------------------*/
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
        /// 外线员业务内容
        /// </summary>
        [Display(Name = "外线员业务内容")]
        public string Business { get; set; }

        /// <summary>
        /// 外线员负责区域简介
        /// </summary>
        [Display(Name = "外线员负责区域简介")]
        public string ResponsibleAreaBrief { get; set; }
    }

    public class SysOnlineUser
    {
        //ContextId
        public string ContextId { get; set; }
        //Online Status
        public string Status { get; set; }
        //用户ID
        public string UserId { get; set; }
        //用户名字
        public string TrueName { get; set; }
        //用户头像
        public string Photo { get; set; }
        //电话号码
        public string PhoneNumber { get; set; }
        //邮件地址
        public string Email { get; set; }
        //组织架构ID
        public string StructId { get; set; }
        //组织架构名字
        public string StructName { get; set; }
        //组织架构排序
        public string Sort { get; set; }
        //职位名字
        public string PosName { get; set; }
    }

    public partial class UserOTM
    {
        public string Id { get; set; }
        public string UserName { get; set; }
    }
}
