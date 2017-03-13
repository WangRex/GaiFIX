using Apps.Locale;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Models.WC
{
    public partial class WC_OfficalAccounts
    {
        [Key]
        public int Id { get; set; }
        public string KEY_Id { get; set; }
        //[NotNullExpression]
        [Display(Name = "WC_OfficalAccounts_OfficalName", ResourceType = typeof(Resource))]
        public string OfficalName { get; set; }
        //[NotNullExpression]
        [Display(Name = "WC_OfficalAccounts_OfficalCode", ResourceType = typeof(Resource))]
        public string OfficalCode { get; set; }
        [Display(Name = "WC_OfficalAccounts_OfficalPhoto", ResourceType = typeof(Resource))]
        public string OfficalPhoto { get; set; }
        [Display(Name = "WC_OfficalAccounts_ApiUrl", ResourceType = typeof(Resource))]
        public string ApiUrl { get; set; }
        [Display(Name = "WC_OfficalAccounts_Token", ResourceType = typeof(Resource))]
        //[NotNullExpression]
        public string Token { get; set; }
        public string AppId { get; set; }
        public string AppSecret { get; set; }
        [Display(Name = "WC_OfficalAccounts_AccessToken", ResourceType = typeof(Resource))]
        public string AccessToken { get; set; }
        [Display(Name = "TitleRemark", ResourceType = typeof(Resource))]
        public string Remark { get; set; }
        [Display(Name = "TitleEnable", ResourceType = typeof(Resource))]
        public string Enable { get; set; }
        [Display(Name = "TitleIsDefault", ResourceType = typeof(Resource))]
        public string IsDefault { get; set; }
        [Display(Name = "TitleCategory", ResourceType = typeof(Resource))]
        public string Category { get; set; }
        [Display(Name = "TitleCreateTime", ResourceType = typeof(Resource))]
        public string CreateTime { get; set; }
        [Display(Name = "TitleCreateBy", ResourceType = typeof(Resource))]
        public string CreateBy { get; set; }
        [Display(Name = "TitleModifyTime", ResourceType = typeof(Resource))]
        public string ModifyTime { get; set; }
        [Display(Name = "TitleModifyBy", ResourceType = typeof(Resource))]
        public string ModifyBy { get; set; }

        public string  OfficalId { get; set; }
        public string OfficalKey { get; set; }
    }
}
