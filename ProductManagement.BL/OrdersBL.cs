using ProductManagement.Entities;
using ProductManagement.Entities.Models;
using ProductManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.BL
{
    public class OrdersBL : IOrderOps
    {
        public IOrderDbOps _orderDbOps;
        public OrdersBL(IOrderDbOps orderDbOps)
        {
            _orderDbOps = orderDbOps;
        }
        public async Task<IEnumerable<OrdersModel>> GetListOfOrders(int userId)
        {
            return await _orderDbOps.GetListOfOrders(userId);
        }
        public async Task<int> PlaceOrders(PlaceOrderModel placeOrder)
        {
            return await _orderDbOps.PlaceOrders(placeOrder);
        }
    }
}
