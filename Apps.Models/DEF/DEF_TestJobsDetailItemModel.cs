using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel;

namespace Apps.Models.DEF
{
    public partial class DEF_TestJobsDetailItem
    {
              [Key]
        public  int Id { get; set; }

        public string KEY_Id { get; set; }
        [DisplayName("版本号")]
        [Required(ErrorMessage = "*")]
        public string VerCode { get; set; }
        [DisplayName("用例编码")]
        [Required(ErrorMessage = "*")]
        public string Code { get; set; }
        [DisplayName("名称")]
        [Required(ErrorMessage = "*")]
        public string Name { get; set; }
        [DisplayName("说明")]
        public string Description { get; set; }
        [DisplayName("测试结果")]
        public  string Result { get; set; }
        [DisplayName("排序")]
        public  string Sort { get; set; }
        [DisplayName("执行序号")]
        public  string ExSort { get; set; }
        [DisplayName("锁定状态")]
        public string Lock { get; set; }

        [DisplayName("开发者")]
        public string Developer { get; set; }
        [DisplayName("测试者")]
        public string Tester { get; set; }
        [DisplayName("实际完成时间")]
        public string FinDt { get; set; }
        [DisplayName("开发完成标志")]
        public  string DevFinFlag { get; set; }
        [DisplayName("请求测试标志")]
        public  string TestRequestFlag { get; set; }

    }
}
