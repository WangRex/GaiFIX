using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Apps.Models.Sys
{
    public partial class SysRoleSysUser
    {
        [Key]
        public int Id { get; set; }
        public string SysUserId { get; set; }
        public string SysRoleId { get; set; }

        public string SysRoleName { get; set; }
    }
}
