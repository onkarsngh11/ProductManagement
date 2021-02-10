using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Entities
{
    public class Orders
    {
        [Key]
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public double? OrderTotal { get; set; }
        public double? ProductSalePrice { get; set; }
        public string ProductImageUrl { get; set; }
        public DateTime PlacedOn { get; set; }
    }
}
