using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Apps.Models.RongHai
{
    public class ChildrenInfo
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "姓名")]
        [Required(ErrorMessage = "必填")]
        public string MyName { get; set; }

        [Display(Name = "乳名")]
        public string BabyName { get; set; }

        [Display(Name = "性别")]
        public string Sex { get; set; }

        [Display(Name = "年龄")]
        public string Age { get; set; }

        [Display(Name = "出生日期")]
        public string Birthday { get; set; }

        [Display(Name = "监护人关系")]
        public string guardian { get; set; }

        [Display(Name = "监护人工作单位")]
        public string WorkUnit { get; set; }

        [Display(Name = "监护人电话")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "号码格式不正确")]
        public string PhoneNumber { get; set; }

        [Display(Name = "入园日期")]
        public string HireDate { get; set; }

        [Display(Name = "现家庭地址")]
        public string Address { get; set; }

        [Display(Name = "年级")]
        public String ClassType { get; set; }

        [Display(Name = "班级")]
        public string ClassName { get; set; }

        [Display(Name = "在校状态")]
        //0未入学 1在学 2退学
        public String State { get; set; }

       //=============================================================================
        [RegularExpression(@"/^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$|^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}([0-9]|X)$/", ErrorMessage = "{0}的格式不正确")]
        [Display(Name = "身份证号码")]
        public string IDCard { get; set; }
        [Display(Name = "户口所在地")]
        public string Registerd { get; set; }
        [Display(Name = "籍贯")]
        public string BrithPlace { get; set; }
        [Display(Name = "监护人2姓名")]
        public string Guardian2 { get; set; }
        [Display(Name = "监护人2与幼儿关系")]
        public string Relation2 { get; set; }
        [Display(Name = "监护人2工作单位")]
        public string WorkUnit2 { get; set; }
        [Display(Name = "监护人2联系电话")]
        public string MumberPhone2 { get; set; }
        [Display(Name = "监护人3姓名")]
        public string Guardian3 { get; set; }
        [Display(Name = "监护人3与幼儿关系")]
        public string Relation3 { get; set; }
        [Display(Name = "监护人3工作单位")]
        public string WorkUnit3 { get; set; }
        [Display(Name = "监护人3联系电话")]
        public string MumberPhone3 { get; set; }
        [Display(Name = "监护人4姓名")]
        public string Guardian4 { get; set; }
        [Display(Name = "监护人4与幼儿关系")]
        public string Relation4 { get; set; }
        [Display(Name = "监护人4工作单位")]
        public string WorkUnit4 { get; set; }
        [Display(Name = "监护人4联系电话")]
        public string MumberPhone4 { get; set; }
        [Display(Name = "有无病史")]
        public string MedicalHistory { get; set; }
        [Display(Name = "有无过敏")]
        public string Allergy { get; set; }
        [Display(Name = "有无传染病")]
        public string Infectious { get; set; }
        [Display(Name = "有无先天性疾病")]
        public string Congential { get; set; }
        [Display(Name = "有无遗传性疾病")]
        public string Genetic { get; set; }
        [Display(Name = "是否残疾")]
        public string Disability { get; set; }
        [Display(Name = "食物禁忌")]
        public string FoodTaboo { get; set; }
        [Display(Name = "是否独生子")]
        public string OnlyChild { get; set; }
        [Display(Name = "是否外来务工人员")]
        public string Migrant { get; set; }
        [Display(Name = "是否低保家庭")]
        public string BasicAllowance { get; set; }
        [Display(Name = "是否单亲家庭")]
        public string SingleParent { get; set; }
        [Display(Name = "其他")]
        public string Others { get; set; }
    }
}