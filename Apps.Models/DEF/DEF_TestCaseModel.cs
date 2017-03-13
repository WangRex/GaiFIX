﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel;

namespace Apps.Models.DEF
{
    public partial class DEF_TestCase
    {
        public const string DEFAULT_MODULEID = "_undefined";//未定义,缺省值
        public const string DEFAULT_MODULEID_TITLE = "未定义";//未定义,缺省值

        [Key]
        public int Id { get; set; }

        public string KEY_Id { get; set; }

        [DisplayName("用例编码")]
        //[NotNullExpression]
        public string Code { get; set; }
        [DisplayName("名称")]
        //[NotNullExpression]
        public string Name { get; set; }
        [DisplayName("说明")]
        public string Description { get; set; }

        
        [DisplayName("排序")]
        public  string Sort { get; set; }
        [DisplayName("分类")]
        //[NotNullExpression]
        public string ModuleId { get; set; }
        [DisplayName("分类名称")]
        public string ModuleIdTitle { get; set; }

        public string state { get; set; }
    }
}
