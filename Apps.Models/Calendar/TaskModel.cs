using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Models.Calendar
{
    public partial class Task
    {
        [Key]
        public int id { get; set; }

        public string UserId { get; set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string className { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public Nullable<DateTime> start { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public Nullable<DateTime> end { get; set; }

        public Boolean allDay { get; set; }
    }
}
