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
        private readonly ProductManagementDbContext _context;

        public CartDbOps(ProductManagementDbContext context)
        {
            _context = context;
        }
        public async Task<int> AddToCart(Cart cart)
        {
            _context.Cart.Add(cart);
            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CartModel>> GetItemsinCart()
        {
            List<CartModel> ListofAllProducts = await (from pd in _context.Products
                                           join ct in _context.Cart on pd.ProductId equals ct.ProductId
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
            Cart cartItemtoremove = await _context.Cart.FirstOrDefaultAsync(p => p.ProductId == Id);
            _context.Cart.Remove(cartItemtoremove);
            return await _context.SaveChangesAsync();
        }
    }
}
