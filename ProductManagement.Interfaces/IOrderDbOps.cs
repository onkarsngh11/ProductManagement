using ProductManagement.Entities;
using ProductManagement.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductManagement.Interfaces
{
    public interface IOrderDbOps
    {
        Task<IEnumerable<OrdersModel>> GetListOfOrders(int userId);
        Task<int> PlaceOrders(PlaceOrderModel placeOrder);
    }
}
