using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Apps.Models.MIS
{
    public partial class MIS_WebIM_Group
    {
        public int Id { get; set; }
        public string KEY_Id { get; set; }
        [Required(ErrorMessage = "*")]
        public string Name { get; set; }
        public string CreateTime { get; set; }
        [Required(ErrorMessage = "*")]
        public string Sort { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
