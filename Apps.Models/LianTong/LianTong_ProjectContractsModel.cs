using System;
using System.ComponentModel.DataAnnotations;

namespace Apps.Models.LianTong
{

    /// <summary>
    /// 合同模型
    /// </summary>
    public class LianTong_ProjectContractsModel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "设计名称")]
        public string designName { get; set; }

        [Display(Name = "设计编号")]
        public string designNum { get; set; }

        [Display(Name = "项目编号")]
        public string projectNum { get; set; }

        [Display(Name = "合同编号")]
        public string contractNum { get; set; }

        [Display(Name = "订单编号")]
        public string orderNum { get; set; }

        [Display(Name = "概算金额")]
        public string outlineCost { get; set; }

        [Display(Name = "送审金额")]
        public string submitCost { get; set; }

        [Display(Name = "审定金额")]
        public string approveCost { get; set; }

        [Display(Name = "合同金额")]
        public string contractCost { get; set; }

        [Display(Name = "所属部门")]
        public string department { get; set; }

        [Display(Name = "所属部门")]
        public string departmentName { get; set; }

        [Display(Name = "甲方管理员")]
        public string ownerAdmin { get; set; }

        [Display(Name = "管理费")]
        public string managementCost { get; set; }

        [Display(Name = "工程类别1")]
        public string projectClass1 { get; set; }

        [Display(Name = "工程类别2")]
        public string projectClass2 { get; set; }

        [Display(Name = "是否分包")]
        public string subContractFlg { get; set; }

        [Display(Name = "分包单位")]
        public string subContractCompany { get; set; }

        [Display(Name = "分包单位负责人")]
        public string subContractAdmin { get; set; }

        [Display(Name = "分包联系电话")]
        public string subContractTel { get; set; }

        [Display(Name = "净价")]
        public string viePrice { get; set; }

        [Display(Name = "税价")]
        public string taxIncrease { get; set; }

        [Display(Name = "含税价")]
        public string taxPrice { get; set; }

        [Display(Name = "发票号")]
        public string invoiceNum { get; set; }

        [Display(Name = "开票日期")]
        public string invoiceDate { get; set; }

        [Display(Name = "有效期限")]
        public string validDate { get; set; }

        [Display(Name = "工程订单列表")]
        public string projectOrderList { get; set; }

        [Display(Name = "流程状态")]
        public string history { get; set; }

        [Display(Name = "历史记录")]
        public string historyRecord { get; set; }

        [Display(Name = "合同状态")]
        public string status { get; set; }

        [Display(Name = "合同编辑状态")]
        public int disable { get; set; }

    }

}