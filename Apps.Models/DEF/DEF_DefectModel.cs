using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel;

namespace Apps.Models.DEF
{
    public partial class DEF_Defect
    {
        [Key]
        public int Id { get; set; }

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
        [DisplayName("用例名称")]
        public string CaseName { get; set; }
        [DisplayName("项目步骤名称")]
        public string Title { get; set; }
        [DisplayName("测试内容")]
        public string TestContent { get; set; }
        [DisplayName("测试结果内容")]
        public string ResultContent { get; set; }
        [DisplayName("创建人")]
        public string Creator { get; set; }
        [DisplayName("创建人")]
        public string CreatorTitle { get; set; }
        [DisplayName("创建日期")]
        public string CrtDt { get; set; }
        [DisplayName("备注")]
        public string Remark { get; set; }
        [DisplayName("接收者")]
        public string Receiver { get; set; }
        [DisplayName("接收者")]
        public string ReceiverTitle { get; set; }
        [DisplayName("发送日期")]
        public string SendDt { get; set; }
        [DisplayName("关闭状态")]
        public  string CloseState { get; set; }
        [DisplayName("关闭人")]
        public string Closer { get; set; }
        [DisplayName("关闭人")]
        public string CloserTitle { get; set; }
        [DisplayName("关闭日期")]
        public string CloseDt { get; set; }
        [DisplayName("消息ID")]
        public string MessageId { get; set; }
        [DisplayName("排序")]
        public  string Sort { get; set; }
        [DisplayName("处理状态")]
        public  string ProcessState { get; set; }
        [DisplayName("处理人")]
        public string Processor { get; set; }
        [DisplayName("处理人")]
        public string ProcessorTitle { get; set; }
        [DisplayName("处理日期")]
        public string ProcessDt { get; set; }

        [DisplayName("错误级别")]
        public  string ErrorLevel { get; set; }

        [DisplayName("复杂度")]
        public  string Complex { get; set; }
        [DisplayName("计划开始日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public string PStartDt { get; set; }

        [DisplayName("计划完成日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public string PEndDt { get; set; }

        [DisplayName("执行人")]
        public string Executor { get; set; }

        [DisplayName("执行人")]
        public string ExecutorTitle { get; set; }


    }
}
