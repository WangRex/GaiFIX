using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apps.Models.Sys
{
    public partial class Account
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string TrueName { get; set; }
        public string Photo { get; set; }
        public string RoleId { get; set; }

        public string RoleName { get; set; }
        public string DepId { get; set; }


    }
}
