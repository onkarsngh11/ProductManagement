using ProductManagement.Entities;
using ProductManagement.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductManagement.Interfaces
{
    public interface ICartDbOps
    {
        Task<IEnumerable<CartModel>> GetItemsinCart();
        Task<int> RemoveItemFromCart(int Id);
        Task<int> AddToCart(Cart cart);
    }
}
