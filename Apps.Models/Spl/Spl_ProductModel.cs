using System;
using System.ComponentModel.DataAnnotations;
using Apps.Models;
using System.Collections.Generic;

namespace Apps.Models.Spl
{
    public partial class Spl_Product
    {
        [Key]
        public int Id { get; set; }
        public string KEY_Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Price { get; set; }
        public string Color { get; set; }
        public string Number { get; set; }
        public string CategoryId { get; set; }
        public string CreateTime { get; set; }
        public string CreateBy { get; set; }
        public string CostPrice { get; set; }
        public string ProductCategory { get; set; }
    }
}
