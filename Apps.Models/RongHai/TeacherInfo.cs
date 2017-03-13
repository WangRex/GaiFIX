using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Apps.Models.RongHai
{
    public class TeacherInfo
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "姓名")]
        [Required(ErrorMessage = "必填")]
        public string MyName { get; set; }

        [Display(Name = "性别")]
        public string Sex { get; set; }

        [Display(Name = "年龄")]
        public string Age { get; set; }

        [Display(Name = "现职称及取得时间")]
        public string Position { get; set; }

        [Display(Name = "所在科室")]
        public string SysDepartment { get; set; }

        [Display(Name = "手机号码")]
        public string MobilePhoneNumber { get; set; }

        [Display(Name = "办公电话")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "号码格式不正确")]
        public string PhoneNumber { get; set; }

        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "{0}的格式不正确")]
        [Display(Name = "邮箱")]
        public string EmailAddress { get; set; }

        [Display(Name = "联系地址")]
        public string Address { get; set; }

        [Display(Name = "入职日期")]
        public string HireDate { get; set; }

        [Display(Name = "状态")]
        public string State { get; set; }

        [Display(Name = "人员性质")]
        public string Property { get; set; }
        //=========================================================================================

        [Display(Name = "民族")]
        public string Nation { get; set; }

        [Display(Name = "籍贯")]
        public string BrithPlace { get; set; }

        [Display(Name = "出生年月")]
        public string BrithDate { get; set; }

        [Display(Name = "婚姻状况")]
        public string MaritalStatus { get; set; }

        [Display(Name = "政治面貌")]
        public string politicalStatus { get; set; }

        [Display(Name = "入党时间")]
        public string PartyTime { get; set; }

        [Display(Name = "户口所在地")]
        public string Registerd { get; set; }

        [Display(Name = "现居住地")]
        public string Residence { get; set; }

        [RegularExpression(@"/^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$|^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}([0-9]|X)$/", ErrorMessage = "{0}的格式不正确")]
        [Display(Name = "身份证号码")]
        public string IDCard { get; set; }

        [Display(Name = "第一学历及毕业时间")]
        public string FirstDegree { get; set; }

        [Display(Name = "最高学历及毕业时间")]
        public string HigherDegree { get; set; }

        [Display(Name = "专业")]
        public string Major { get; set; }

        [Display(Name = "养老保险账号")]
        public string EndowmentInsurance { get; set; }

        [Display(Name = "养老保险参保时间")]
        public string EndowmentTime { get; set; }

        [Display(Name = "养老保险账号")]
        public string MedicalInsurance { get; set; }

        [Display(Name = "医疗保险参保时间")]
        public string MedicalTime { get; set; }

        [Display(Name = "公积金账号")]
        public string PublicFunds { get; set; }

        [Display(Name = "公积金缴纳开始时间")]
        public string PublicFundTime { get; set; }

        [Display(Name = "教师职格证")]
        public string TeacherCertificateId { get; set; }

        [Display(Name = "普通话级别")]
        public string MandarinLevel { get; set; }

        [Display(Name = "健康状况")]
        public string HealthStatus { get; set; }

        [Display(Name = "特长")]
        public string SpecialSkill { get; set; }

        [Display(Name = "家庭主要成员1姓名")]
        public string FamilyName1 { get; set; }

        [Display(Name = "家庭成员1与本人关系")]
        public string Relation1 { get; set; }

        [Display(Name = "家庭成员1工作单位")]
        public string WorkUnit1 { get; set; }

        [Display(Name = "家庭成员1联系电话")]
        public string MumberPhone1 { get; set; }

        [Display(Name = "家庭主要成员2姓名")]
        public string FamilyName2 { get; set; }

        [Display(Name = "家庭成员2与本人关系")]
        public string Relation2 { get; set; }

        [Display(Name = "家庭成员2工作单位")]
        public string WorkUnit2 { get; set; }

        [Display(Name = "家庭成员2联系电话")]
        public string MumberPhone2 { get; set; }

        [Display(Name = "家庭主要成员3姓名")]
        public string FamilyName3 { get; set; }

        [Display(Name = "家庭成员3与本人关系")]
        public string Relation3 { get; set; }

        [Display(Name = "家庭成员3工作单位")]
        public string WorkUnit3 { get; set; }

        [Display(Name = "家庭成员3联系电话")]
        public string MumberPhone3 { get; set; }

        [Display(Name = "家庭主要成员4姓名")]
        public string FamilyName4 { get; set; }

        [Display(Name = "家庭成员4与本人关系")]
        public string Relation4 { get; set; }

        [Display(Name = "家庭成员4工作单位")]
        public string WorkUnit4 { get; set; }

        [Display(Name = "家庭成员4联系电话")]
        public string MumberPhone4 { get; set; }

        [Display(Name = "个人学历简历")]
        public string StudyResume { get; set; }

        [Display(Name = "个人工作简历")]
        public string WorkResume { get; set; }

        [Display(Name = "获奖情况")]
        public string Award { get; set; }

        [Display(Name = "备注")]
        public string Comments { get; set; }
    }
}