using System;
using System.ComponentModel.DataAnnotations;

namespace Apps.Models.LianTong
{

    /// <summary>
    /// 工程详情
    /// </summary>
    public class LianTong_ProjectModel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "工程编号")]
        public string projectNum { get; set; }

        [Display(Name = "工程属地")]
        public string projectLocatioin { get; set; }

        [Display(Name = "工程类别")]
        public string projectClass { get; set; }

        [Display(Name = "局向")]
        public string officeDirect { get; set; }

        [Display(Name = "建设管理员")]
        public string admin { get; set; }

        [Display(Name = "工程负责人")]
        public string projectManagement { get; set; }

        [Display(Name = "专业")]
        public string projectPro{ get; set; }

        [Display(Name = "专业2")]
        public string projectPro2 { get; set; }

        [Display(Name = "单项工程名称")]
        public string singleProjectName { get; set; }

        [Display(Name = "建安费")]
        public string outlineCost { get; set; }

        [Display(Name = "人工费")]
        public string laborCost { get; set; }

        [Display(Name = "材料费")]
        public string materialsCost { get; set; }

        [Display(Name = "是否合作")]
        public string cooFlg { get; set; }

        [Display(Name = "合作单位")]
        public string cooCompany { get; set; }

        [Display(Name = "所属部门")]
        public string department { get; set; }

        [Display(Name = "所属部门")]
        public string departmentName { get; set; }

        [Display(Name = "工程状态")]
        public string status { get; set; }

        [Display(Name = "完成情况")]
        public string overStatus { get; set; }

        [Display(Name = "合同编号")]
        public string contractNum { get; set; }

        [Display(Name = "备注")]
        public string comment { get; set; }
    }

}