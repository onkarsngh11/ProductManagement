using System;
using System.ComponentModel;

namespace ProductManagement.Entities.Models
{
    public class OrdersModel
    {
        [DisplayName("Product Image")]
        public string Image { get; set; }
        [DisplayName("Product Name")]
        public string ProductName { get; set; }
        [DisplayName("Description")]
        public string ProductDescription { get; set; }
        [DisplayName("Order Total")]
        public double? OrderTotal { get; set; }
        public DateTime PlacedOn { get; set; }
    }
}
