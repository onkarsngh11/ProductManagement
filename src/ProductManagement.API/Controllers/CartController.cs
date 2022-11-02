using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Entities.Models;
using ProductManagement.Entities;
using ProductManagement.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ProductManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]

    [Authorize(Roles = "User,Admin")]
    public class CartController : ControllerBase
    {
        private readonly ICartOps _cartOps;
        public CartController(ICartOps cartOps)
        {
            _cartOps = cartOps;
        }
        [HttpGet]
        public ActionResult Index()
        {
            return Content("Hi from Cart!");
        }
        [HttpGet]
        [Route("GetItemsInCart")]
        public async Task<IEnumerable<CartModel>> GetItemsInCart()
        {
            return await _cartOps.GetItemsinCart();
        }

        [HttpPost]
        [Route("AddToCart")]
        public async Task<int> AddToCart(Cart cart)
        {
            return await _cartOps.AddToCart(cart);
        }

        [HttpDelete]
        [Route("RemoveItemFromCart")]
        public async Task<int> RemoveItemFromCart(int Id)
        {
            return await _cartOps.RemoveItemFromCart(Id);
        }
    }
}