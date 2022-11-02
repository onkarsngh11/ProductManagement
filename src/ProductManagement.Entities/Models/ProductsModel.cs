using Microsoft.AspNetCore.Http;
using ProductManagement.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace  ProductManagement.Entities.Models
{
    public class ProductsModel
    {
        public int ProductId { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Description { get; set; }
        public double? Price { get; set; }
        public double? SalePrice { get; set; }
        public string Image { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
        public IFormFile UploadedFiles { get; set; }
    }
}
