using System;
using System.ComponentModel.DataAnnotations;

namespace Apps.Models.FIX
{
    public class FIX_BaseModels
    {
        //------------------------每个表必须有的项目-----------------------------------------
        [ScaffoldColumn(false)]
        [Display(Name = "创建时间")]
        public string CreateTime { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "创建人")]
        public string CreatePerson { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "编辑时间")]
        public string UpdateTime { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "编辑人")]
        public string UpdatePerson { get; set; }
    }
}