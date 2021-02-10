using Microsoft.EntityFrameworkCore;
using ProductManagement.Entities;
using ProductManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using ProductManagement.Entities.Models;

namespace ProductManagement.DAL
{
    public class CartDbOps : ICartDbOps
    {
        public async Task<int> AddToCart(Cart cart)
        {
            using ProductManagementDbContext _context = new ProductManagementDbContext();
            _context.Cart.Add(cart);
            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CartModel>> GetItemsinCart()
        {
            using ProductManagementDbContext context = new ProductManagementDbContext();
            List<CartModel> ListofAllProducts = await (from pd in context.Products
                                           join ct in context.Cart on pd.ProductId equals ct.ProductId
                                           orderby pd.ProductId
                                           select new CartModel
                                           {
                                               UserId = ct.UserId,
                                               ProductId = ct.ProductId,
                                               Name = pd.Name,
                                               Description = pd.Description,
                                               Image = pd.Image,
                                               ImageUrl = pd.ImageUrl,
                                               Price = pd.SalePrice
                                           }).ToListAsync();
            return ListofAllProducts;
        }

        public async Task<int> RemoveItemFromCart(int Id)
        {
            using ProductManagementDbContext context = new ProductManagementDbContext();
            Cart cartItemtoremove = await context.Cart.FirstOrDefaultAsync(p => p.ProductId == Id);
            context.Cart.Remove(cartItemtoremove);
            return await context.SaveChangesAsync();
        }
    }
}
