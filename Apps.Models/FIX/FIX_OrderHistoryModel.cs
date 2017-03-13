using System.ComponentModel.DataAnnotations;

namespace Apps.Models.FIX
{
    public class FIX_OrderHistoryModel : FIX_BaseModels
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        [Display(Name = "订单ID")]
        public int Order_ID { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        [Display(Name = "订单状态")]
        public string Status { get; set; }

    }
}
