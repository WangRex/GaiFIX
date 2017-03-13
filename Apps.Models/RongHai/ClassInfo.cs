using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Apps.Models.RongHai
{
    public class ClassInfo
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "班级名称")]
        [Required(ErrorMessage = "必填")]
        public string ClassName { get; set; }

        [Display(Name = "班主任1姓名")]
        public string TeacherName { get; set; }

        [Display(Name = "班主任1电话")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "号码格式不正确")]
        public string PhoneNumber { get; set; }

        [Display(Name = "班主任2姓名")]
        public string TeacherName2 { get; set; }

        [Display(Name = "班主任2电话")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "号码格式不正确")]
        public string PhoneNumber2 { get; set; }

        [Display(Name = "保育员姓名")]
        public string TeacherName3 { get; set; }

        [Display(Name = "保育员电话")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "号码格式不正确")]
        public string PhoneNumber3 { get; set; }

        [Display(Name = "班级成立日期")]
        public string RegeditDate { get; set; }

        [Display(Name = "班级介绍")]
        public string Document { get; set; }

        [Display(Name = "班级额定人数")]
        public string KidCount { get; set; }

        [Display(Name = "班级分类")]
        public String ClassType { get; set; }

        [Display(Name = "班级状态")]
        public String State { get; set; }
    }
}