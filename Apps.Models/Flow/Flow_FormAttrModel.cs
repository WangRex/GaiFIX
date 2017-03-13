using System;
using System.ComponentModel.DataAnnotations;
using Apps.Models;
namespace Apps.Models.Flow
{
    public partial class Flow_FormAttr
    {
        [Key]
        public int Id { get; set; }
        public string KEY_Id { get; set; }
        [MaxWordsExpression(50)]
        [Display(Name = "字段标题")]
        public string Title { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "字段英文名称")]
        public string Name { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "类型")]
        public string AttrType { get; set; }//文本,日期,数字,多行文本

        [MaxWordsExpression(500)]
        [Display(Name = "校验脚本")]
        public string CheckJS { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "所属类别")]
        public string TypeId { get; set; }

        public  string TypeName { get; set; }

        [Display(Name = "创建时间")]
        public string CreateTime { get; set; }
    }
}
