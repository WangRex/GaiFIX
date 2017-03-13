using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace Apps.Models.JOB
{
    public partial class JOB_TASKJOBS_LOG
   {
        [Key]
        public int Id { get; set; }
        public string KEY_Id { get; set; }
        [DisplayName("日志序号")]
       [Required(ErrorMessage = "*")]
       public string itemID { get; set; }
       [DisplayName("任务序号")]
       [Required(ErrorMessage = "*")]
       public string sno { get; set; }
       [DisplayName("任务名称")]
       [Required(ErrorMessage = "*")]
       public string taskName { get; set; }
       [DisplayName("任务ID")]
       [Required(ErrorMessage = "*")]
       public string IdR { get; set; }
       [DisplayName("执行时间")]
       public string executeDt { get; set; }
       [DisplayName("执行步骤")]
       public string executeStep { get; set; }
       [DisplayName("结果")]
       public string result { get; set; }
    }
}
