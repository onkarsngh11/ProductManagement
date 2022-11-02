using ProductManagement.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductManagement.Interfaces
{
    public interface IProductOps
    {
        Task<Products> GetProductById(int Id);
        Task<IEnumerable<Products>> GetListOfAllProducts();
        Task<int> AddProduct(Products Product);
        Task<int> DeleteProduct(int Id);
        Task<int> UpdateProduct(Products product);
        Task<int> UpdateProductPrice(Products product);
        Task<IEnumerable<Products>> SearchProducts(Products models);
    }
}
