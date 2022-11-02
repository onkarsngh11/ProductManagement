using ProductManagement.Interfaces;
using ProductManagement.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductManagement.BL
{
    public class ProductsBL : IProductOps
    {
        public IProductDbOps _productDbOps;
        public ProductsBL(IProductDbOps productDbOps)
        {
            _productDbOps = productDbOps;
        }
        
        public async Task<int> AddProduct(Products Product)
        {
            return await _productDbOps.AddProduct(Product);
        }

        public async Task<int> DeleteProduct(int Id)
        {
            return await _productDbOps.DeleteProduct(Id);
        }

        public async Task<IEnumerable<Products>> GetListOfAllProducts()
        {
            return await _productDbOps.GetListOfAllProducts();
        }

        public async Task<Products> GetProductById(int Id)
        {
            return await _productDbOps.GetProductById(Id);
        }

        public async Task<int> UpdateProductPrice(Products product)
        {
            return await _productDbOps.UpdateProductPrice(product);
        }

        public async Task<int> UpdateProduct(Products product)
        {
            return await _productDbOps.UpdateProduct(product);
        }
        public async Task<IEnumerable<Products>> SearchProducts(Products product)
        {
            return await _productDbOps.SearchProducts(product);
        }
    }
}
