using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Entities;
using ProductManagement.Interfaces;

namespace ProductManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductOps _productops;
        public ProductsController(IProductOps productOps)
        {
            _productops = productOps;
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index()
        {
            return Content("Hi from Product!");
        }

        [Authorize(Roles = "Users,Admin")]
        [HttpGet]
        [Route("GetProductById")]
        public async Task<Products> GetProductById(int Id)
        {
            return await _productops.GetProductById(Id);
        }

        [HttpGet]
        [Route("GetListOfAllProducts")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IEnumerable<Products>> GetListOfAllProducts()
        {
            return await _productops.GetListOfAllProducts();
        }

        [HttpPost]
        [Route("AddProduct")]
        [Authorize(Policy = "Admin")]
        public async Task<int> AddProduct(Products product)
        {
            return await _productops.AddProduct(product);
        }

        [HttpPost]
        [Route("UpdateProduct")]
        [Authorize(Policy = "Admin")]
        public async Task<int> UpdateProduct(Products product)
        {
            return await _productops.UpdateProduct(product);
        }

        [HttpDelete]
        [Route("DeleteProduct")]
        [Authorize(Policy = "Admin")]
        public async Task<int> DeleteProduct(int id)
        {
            return await _productops.DeleteProduct(id);
        }

    }
}
