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
        private readonly ProductManagementDbContext context;

        public PMDbOps(ProductManagementDbContext dbContext)
        {
            context = dbContext;
        }
        public async Task<int> AddProduct(Products Product)
        {
            context.Products.Add(Product);
            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteProduct(int Id)
        {
            Products productIdtoDelete = await context.Products.FindAsync(Id);
            context.Products.Remove(productIdtoDelete);
            return await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Products>> GetListOfAllProducts()
        {
            List<Products> ListofAllProducts = await context.Products.ToListAsync();
            return ListofAllProducts;
        }

        public async Task<Products> GetProductById(int Id)
        {
            Products Product = await context.Products.FirstOrDefaultAsync(p => p.ProductId == Id);
            return Product;
        }

        public async Task<IEnumerable<Products>> SearchProducts(Products product)
        {
            List<Products> ListOfFilteredProducts = await context.Products.Where(p => p.Description == product.Description).ToListAsync();
            return ListOfFilteredProducts;
        }

        public async Task<int> UpdateProduct(Products product)
        {
            context.Products.Update(product);
            return await context.SaveChangesAsync();
        }

        public async Task<int> UpdateProductPrice(Products product)
        {
            Products productIdtoUpdate = await context.Products.FindAsync(product.ProductId);
            productIdtoUpdate.Price = product.Price;
            return await context.SaveChangesAsync();
        }
    }
}
