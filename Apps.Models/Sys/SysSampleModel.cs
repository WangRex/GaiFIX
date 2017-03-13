using System;
using System.ComponentModel.DataAnnotations;
using Apps.Models;
using Apps.Locale;
namespace Apps.Models.Sys
{
    public partial class SysSample
    {
        [Key]
        public int Id { get ; set; }
        public string KEY_Id { get; set; }
        //[NotNullExpression]
        [MaxWordsExpression(50)]
        [Display(Name = "SysSample_Name", ResourceType = typeof(Resource))]
        public string Name { get; set; }

        [Display(Name = "Age")]
        public string Age { get; set; }

        [Display(Name = "Bir")]
        public string Bir { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "Photo")]
        public string Photo { get; set; }

        [MaxWordsExpression(16)]
        [Display(Name = "Note")]
        public string Note { get; set; }

        [Display(Name = "CreateTime")]
        public string CreateTime { get; set; }

    }
}
