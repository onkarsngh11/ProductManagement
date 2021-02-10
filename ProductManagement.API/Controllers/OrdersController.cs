using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Entities;
using ProductManagement.Entities.Models;
using ProductManagement.Interfaces;

namespace ProductManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "User,Admin")]
    public class OrdersController : ControllerBase
    {

        private readonly IOrderOps _productops;
        public OrdersController(IOrderOps productOps)
        {
            _productops = productOps;
        }
        [HttpGet]
        public ActionResult Index()
        {
            return Content("Hi from Orders!");
        }
        [HttpGet]
        [Route("GetListOfOrders")]
        public async Task<IEnumerable<OrdersModel>> GetListOfOrders(int userId)
        {
            return await _productops.GetListOfOrders(userId);
        }
        [HttpPost]
        [Route("PlaceOrder")]
        public async Task<int> PlaceOrder(PlaceOrderModel placeOrder)
        {
            return await _productops.PlaceOrders(placeOrder);
        }
    }
}