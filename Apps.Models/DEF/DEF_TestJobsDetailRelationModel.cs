using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel;

namespace Apps.Models.DEF
{
    public partial class DEF_TestJobsDetailRelation
    {
              [Key]
        public  int Id { get; set; }

        public string KEY_Id { get; set; }

        [DisplayName("版本号")]
        [Required(ErrorMessage = "*")]
        public string VerCode { get; set; }
        [DisplayName("主用例编码")]
        [Required(ErrorMessage = "*")]
        public string PCode { get; set; }
        [DisplayName("子用例编码")]
        [Required(ErrorMessage = "*")]
        public string CCode { get; set; }
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
    }
}
