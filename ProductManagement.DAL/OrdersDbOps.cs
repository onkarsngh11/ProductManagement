using Microsoft.EntityFrameworkCore;
using ProductManagement.Entities;
using ProductManagement.Entities.Models;
using ProductManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace ProductManagement.DAL
{
    public class OrdersDbOps : IOrderDbOps
    {
        public async Task<IEnumerable<OrdersModel>> GetListOfOrders(int userId)
        {
            using ProductManagementDbContext context = new ProductManagementDbContext();
            OrdersModel ordersModel = new OrdersModel();
            List<OrdersModel> ListofAllProducts = await (from orders in context.Orders
                                                         where orders.UserId == userId
                                                         orderby orders.OrderId
                                                         select new OrdersModel
                                                         {
                                                             Image = orders.ProductImageUrl,
                                                             ProductDescription = orders.ProductDescription,
                                                             ProductName = orders.ProductName,
                                                             OrderTotal = orders.OrderTotal,
                                                             PlacedOn = orders.PlacedOn
                                                         }).ToListAsync();
            return ListofAllProducts;
        }

        public async Task<int> PlaceOrders(PlaceOrderModel placeOrder)
        {
            using ProductManagementDbContext context = new ProductManagementDbContext();
            string ids = string.Empty;
            foreach (string Id in placeOrder.IDs)
            {
                Products product = new Products();
                Products productbyId = await context.Products.FirstOrDefaultAsync(p => p.ProductId == Convert.ToInt32(Id));
                Orders order = new Orders
                {
                    PlacedOn = DateTime.Now,
                    UserId = placeOrder.UserId,
                    ProductName = productbyId.Name,
                    ProductImageUrl = productbyId.ImageUrl,
                    ProductSalePrice = productbyId.SalePrice,
                    OrderTotal = productbyId.SalePrice,
                    ProductDescription = productbyId.Description,
                    ProductId = Convert.ToInt32(Id)
                };
                await context.Orders.AddAsync(order);
            }
            //remove from cart
            foreach (string Id in placeOrder.IDs)
            {
                List<Cart> cartItemtoremove = await context.Cart.Where(p => p.ProductId == Convert.ToInt32(Id)).ToListAsync();
                context.Cart.RemoveRange(cartItemtoremove);
            }
            return await context.SaveChangesAsync();
        }
    }
}
