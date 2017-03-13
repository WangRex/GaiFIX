using System.ComponentModel.DataAnnotations;

namespace Apps.Models.FIX
{
    public class FIX_OrderModel : FIX_BaseModels
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        [Display(Name = "用户姓名")]
        public string Name { get; set; }

        /// <summary>
        /// 所在省市
        /// </summary>
        [Display(Name = "所在省市")]
        public string Province { get; set; }

        /// <summary>
        /// 所在城市
        /// </summary>
        [Display(Name = "所在城市")]
        public string City { get; set; }

        /// <summary>
        /// 所在辖区
        /// </summary>
        [Display(Name = "所在辖区")]
        public string Area { get; set; }

        /// <summary>
        /// 订单时间
        /// </summary>
        [Display(Name = "订单时间")]
        public string Time { get; set; }

        /// <summary>
        /// 订单地址
        /// </summary>
        [Display(Name = "订单地址")]
        public string Address { get; set; }

        /// <summary>
        /// 订单匹配地址
        /// </summary>
        [Display(Name = "订单匹配地址")]
        public string MatchingAddress { get; set; }

        /// <summary>
        /// 订单手机号
        /// </summary>
        [Display(Name = "订单手机号")]
        public string Phone { get; set; }

        /// <summary>
        /// 订单座机号
        /// </summary>
        [Display(Name = "订单座机号")]
        public string Tel { get; set; }

        /// <summary>
        /// 订单维修类型
        /// </summary>
        [Display(Name = "订单维修类型")]
        public string Type { get; set; }

        /// <summary>
        /// 订单状态
        /// 新订单/待接单/处理中/待评价/已完成
        /// </summary>
        [Display(Name = "订单状态")]
        public string Status { get; set; }

        /// <summary>
        /// 订单描述
        /// </summary>
        [Display(Name = "订单描述")]
        public string Description { get; set; }

        /// <summary>
        /// 外线员
        /// </summary>
        [Display(Name = "外线员")]
        public string OTM_ID { get; set; }

        /// <summary>
        /// 外线员姓名
        /// </summary>
        [Display(Name = "外线员姓名")]
        public string OTM_Name { get; set; }

        /// <summary>
        /// 评价ID
        /// </summary>
        [Display(Name = "评价ID")]
        public int Assessment_ID { get; set; }

        /// <summary>
        /// 所属部门
        /// </summary>
        [Display(Name = "所属部门")]
        public string department { get; set; }

        /// <summary>
        /// 所属部门
        /// </summary>
        [Display(Name = "所属部门")]
        public string departmentName { get; set; }

        /// <summary>
        /// 属地
        /// </summary>
        [Display(Name = "属地")]
        public string possession { get; set; }

        /// <summary>
        /// 属地
        /// </summary>
        [Display(Name = "属地")]
        public string possessionName { get; set; }
    }
}