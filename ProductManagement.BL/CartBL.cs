using ProductManagement.Entities;
using ProductManagement.Entities.Models;
using ProductManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.BL
{
    public class CartBL : ICartOps
    {
        public ICartDbOps _cartDbOps;
        public CartBL(ICartDbOps cartDbOps)
        {
            _cartDbOps = cartDbOps;
        }
        public async Task<int> AddToCart(Cart cart)
        {
            return await _cartDbOps.AddToCart(cart);
        }

        public Task<IEnumerable<CartModel>> GetItemsinCart()
        {
            return _cartDbOps.GetItemsinCart();
        }

        public Task<int> RemoveItemFromCart(int Id)
        {
            return _cartDbOps.RemoveItemFromCart(Id);
        }
    }
}
