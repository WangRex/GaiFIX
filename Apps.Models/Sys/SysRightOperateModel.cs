using System;
using System.ComponentModel.DataAnnotations;
using Apps.Models;
namespace Apps.Models.Sys
{
    public partial class SysRightOperate
    {
        [Key]
        public int Id { get; set; }
        public string KEY_Id { get; set; }
        public string RightId { get; set; }
        public string KeyCode { get; set; }
        public string IsValid { get; set; }

    }
}
