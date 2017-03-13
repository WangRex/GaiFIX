using System.ComponentModel.DataAnnotations;

namespace Apps.Models.FIX
{
    /// <summary>
    /// 客户数据模型
    /// </summary>
    public class FIX_CustomerModel : FIX_BaseModels
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "姓名")]
        public string Name { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [Display(Name = "电话")]
        public string Phone { get; set; }

        /// <summary>
        /// 座机
        /// </summary>
        [Display(Name = "座机")]
        public string Tel { get; set; }

        /// <summary>
        /// 住址
        /// </summary>
        [Display(Name = "住址")]
        public string Address { get; set; }

        /// <summary>
        /// ONU
        /// </summary>
        [Display(Name = "ONU")]
        public string ONU { get; set; }

    }
}
