using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel;
namespace Apps.Models.DEF
{
   public partial class DEF_TestCaseSteps
   {
 
             [Key]

       public int Id { get; set; }
        [DisplayName("ID")]
        public string KEY_Id { get; set; }

        [DisplayName("用例编码")]
       public string Code { get; set; }
       [DisplayName("标题")]
       [Required(ErrorMessage = "*")]
       public string Title { get; set; }
       [DisplayName("测试内容")]
       public string TestContent { get; set; }
       [DisplayName("状态")]
       public  string state { get; set; }
       [DisplayName("排序")]
       public  string sort { get; set; }
        public string ItemID { get; set; }
    }
}
