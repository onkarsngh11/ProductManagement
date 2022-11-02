using System;
using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Entities
{
    public class Products
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double? Price { get; set; }
        public double? SalePrice { get; set; }
        public string Image { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
    }
}
