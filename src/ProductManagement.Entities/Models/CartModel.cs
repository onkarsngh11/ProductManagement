using ProductManagement.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace  ProductManagement.Entities.Models
{
    public class CartModel
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        [MaxLength(50, ErrorMessage = "Max length is 50 characters")]
        public string Name { get; set; }
        [MaxLength(200, ErrorMessage = "Max length is 200 characters")]
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Image { get; set; }
        public double? Price { get; set; }
    }
}
