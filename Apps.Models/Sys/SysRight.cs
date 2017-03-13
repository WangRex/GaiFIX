using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Apps.Models.Sys
{

    public partial class SysRight
    {


        [Key]
        public int Id { get; set; }

        public string MopId { get; set; }
        public string ModuleId { get; set; }
        public string RoleId { get; set; }
        public string IsValid { get; set; }
        public string KeyCode { get; set; }
}
}
