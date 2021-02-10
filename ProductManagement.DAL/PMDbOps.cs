using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Entities;
using ProductManagement.Interfaces;

namespace ProductManagement.DAL
{
    public class PMDbOps : IProductDbOps
    {
        public async Task<int> AddProduct(Products Product)
        {
            using ProductManagementDbContext context = new ProductManagementDbContext();
            context.Products.Add(Product);
            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteProduct(int Id)
        {
            using ProductManagementDbContext context = new ProductManagementDbContext();
            Products productIdtoDelete = await context.Products.FindAsync(Id);
            context.Products.Remove(productIdtoDelete);
            return await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Products>> GetListOfAllProducts()
        {
            using ProductManagementDbContext context = new ProductManagementDbContext();
            List<Products> ListofAllProducts = await context.Products.ToListAsync();
            return ListofAllProducts;
        }

        public async Task<Products> GetProductById(int Id)
        {
            using ProductManagementDbContext context = new ProductManagementDbContext();
            Products Product = await context.Products.FirstOrDefaultAsync(p => p.ProductId == Id);
            return Product;
        }

        public async Task<IEnumerable<Products>> SearchProducts(Products product)
        {
            using ProductManagementDbContext context = new ProductManagementDbContext();
            List<Products> ListOfFilteredProducts = await context.Products.Where(p => p.Description == product.Description).ToListAsync();
            return ListOfFilteredProducts;
        }

        public async Task<int> UpdateProduct(Products product)
        {
            using ProductManagementDbContext context = new ProductManagementDbContext();
            context.Products.Update(product);
            return await context.SaveChangesAsync();
        }

        public async Task<int> UpdateProductPrice(Products product)
        {
            using ProductManagementDbContext context = new ProductManagementDbContext();
            Products productIdtoUpdate = await context.Products.FindAsync(product.ProductId);
            productIdtoUpdate.Price = product.Price;
            return await context.SaveChangesAsync();
        }
    }
}
