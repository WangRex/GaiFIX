using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel;

namespace Apps.Models.DEF
{
    public partial class DEF_TestJobsDetail
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
        [RegularExpression(@"[\w\W]{0,2000}", ErrorMessage = "{0}为1－2000字。")] //此默认生成的正则为允许任意字符，请根据业务逻辑修改
        public string Description { get; set; }
        [DisplayName("结果")]
        public  string Result { get; set; }
        [DisplayName("排序")]
        public  string Sort { get; set; }
    }
}
