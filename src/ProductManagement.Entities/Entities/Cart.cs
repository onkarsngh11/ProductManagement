using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManagement.Entities
{
    public class Cart
    {
        public Guid CartId { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
    }
}
