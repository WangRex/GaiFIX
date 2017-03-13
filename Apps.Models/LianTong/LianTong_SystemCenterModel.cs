using System;
using System.ComponentModel.DataAnnotations;

namespace Apps.Models.LianTong
{

    /// <summary>
    /// 工程详情
    /// </summary>
    public class LianTong_SystemCenterModel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "集成中心负责人")]
        public string leaderName { get; set; }

        [Display(Name = "项目名称")]
        public string projectName { get; set; }

        [Display(Name = "属地")]
        public string projectAttribution { get; set; }

        [Display(Name = "区域")]
        public string projectArea { get; set; }

        [Display(Name = "项目类别")]
        public string projectType { get; set; }

        [Display(Name = "联通公司客户经理")]
        public string AccountManager { get; set; }

        [Display(Name = "用户地址")]
        public string adress { get; set; }

        [Display(Name = "用户联系人")]
        public string contactPeople { get; set; }

        [Display(Name = "用户联系方式")]
        public string contactWay { get; set; }

        [Display(Name = "设备选型")]
        public string equipmentType { get; set; }

        [Display(Name = "设备经销商")]
        public string equipmentDealer { get; set; }

        [Display(Name = "施工部门")]
        public string constructionDepartment { get; set; }

        [Display(Name = "合同金额")]
        public Double? contractCost { get; set; }

        [Display(Name = "设备采购金额")]
        public Double? equipmentCost { get; set; }

        [Display(Name = "施工费用")]
        public Double? projectCost { get; set; }

        [Display(Name = "维保部门")]
        public string maintenanceDepartment { get; set; }

        [Display(Name = "维保年限")]
        public string maintenancePeriod { get; set; }

        [Display(Name = "合同签订时间")]
        public string contractStartDate { get; set; }

        [Display(Name = "合同结束时间")]
        public string contractEndDate { get; set; }

        //===================================================
        [Display(Name = "客户经理电话")]
        public string AccountManagerTel { get; set; }

        [Display(Name = "联通公司客户")]
        public string UnicomAccount { get; set; }

        [Display(Name = "联通公司系统支持")]
        public string SystemSupport { get; set; }

        [Display(Name = "设备供应商联系人")]
        public string equipmentDealerContacts { get; set; }

        [Display(Name = "设备供应商电话")]
        public string equipmentDealerTel { get; set; }

        [Display(Name = "备注")]
        public string Comments { get; set; }

        [Display(Name = "附件")]
        public string UpLoad { get; set; }
    }

}