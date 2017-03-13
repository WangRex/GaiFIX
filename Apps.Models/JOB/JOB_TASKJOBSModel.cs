using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace Apps.Models.JOB
{
    public partial class JOB_TASKJOBS
    {
        [Key]
        public int Id { get; set; }
        public string KEY_Id { get; set; }
        [DisplayName("序号")]
        [Required(ErrorMessage = "*")]
        public string sno { get; set; }
        [DisplayName("任务名称")]
        [Required(ErrorMessage = "*")]
        public string taskName { get; set; }
        public  string stateTitle { get; set; }//'状态',
        public  string action { get; set; }// '操作',
        [DisplayName("任务ID")]
        [Required(ErrorMessage = "*")]
        public string IdR { get; set; }
        [DisplayName("任务标题")]
        public string taskTitle { get; set; }
        [DisplayName("执行指令")]
        public string taskCmd { get; set; }
        [DisplayName("创建时间")]
        public string crtDt { get; set; }
        [DisplayName("状态")]
        public  string state { get; set; }//0准备，1成功，2关闭，3挂起,4重启
        [DisplayName("创建人")]
        public string creator { get; set; }
        [DisplayName("过程")]
        public string procName { get; set; }
        [DisplayName("过程参数")]
        public string procParams { get; set; }
    }
}
