using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel;

namespace Apps.Models.DEF
{
    public partial class DEF_TestJobsDetailSteps
    {
              [Key]
        public  int Id { get; set; }

        public string KEY_Id { get; set; }

        [DisplayName("ID")]
        [Required(ErrorMessage = "*")]
        public string ItemID { get; set; }

        [DisplayName("版本号")]
        [Required(ErrorMessage = "*")]
        public string VerCode { get; set; }
        [DisplayName("用例编码")]
        [Required(ErrorMessage = "*")]
        public string Code { get; set; }
        [DisplayName("测试步骤")]
        [Required(ErrorMessage = "*")]
        public string Title { get; set; }
        [DisplayName("测试内容")]
        public string TestContent { get; set; }
        [DisplayName("结果")]
        public  string Result { get; set; }
        [DisplayName("排序")]
        public  string Sort { get; set; }
        [DisplayName("测试结果内容")]
        public string ResultContent { get; set; }
        [DisplayName("执行序号")]
        public  string ExSort { get; set; }
        [DisplayName("步骤类型")]
        public string StepType { get; set; }

        [DisplayName("测试人")]
        public string Tester { get; set; }
        [DisplayName("测试时间")]
        public string TestDt { get; set; }

        [DisplayName("开发者")]
        public string Developer { get; set; }
        [DisplayName("计划开始时间")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public string PlanStartDt { get; set; }
        [DisplayName("计划完成时间")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public string PlanEndDt { get; set; }
        [DisplayName("实际完成时间")]
        public string FinDt { get; set; }
        [DisplayName("开发完成标志")]
        public  string DevFinFlag { get; set; }
        [DisplayName("请求测试标志")]
        public  string TestRequestFlag { get; set; }
        [DisplayName("用例名称")]
        public string CodeName { get; set; }
    }
}
