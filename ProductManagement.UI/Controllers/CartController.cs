using Microsoft.AspNetCore.Mvc;
using ProductManagement.Entities;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Formatting;
using System.Collections.Generic;
using System.Text.Json;
using ProductManagement.Entities.Models;
using ProductManagement.Interfaces;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace ProductManagement.UI.Controllers
{
    public class CartController : BaseController
    {
        private readonly IHttpClientHelper _httpClientHelper;
        public CartController(IHttpClientHelper httpClientHelper)
        {
            _httpClientHelper = httpClientHelper;
        }
        // GET: Cart
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> Index()        
        {
            IEnumerable<CartModel> CartItems = await GetItemsInCart();
            ViewBag.TotalProducts = GetCountofItemsinCart(CartItems, out double? totalprice);
            ViewBag.TotalPrice = totalprice;
            return View(CartItems);
        }
        public async Task<IEnumerable<CartModel>> GetItemsInCart()
        {
            using HttpClient client = await _httpClientHelper.GetHttpClientAsync();
            using HttpResponseMessage response = await client.GetAsync(_httpClientHelper.GetApiUrl("GetItemsInCart"));
            System.IO.Stream responseBody = response.Content.ReadAsStreamAsync().Result;
            IEnumerable<CartModel> list = await JsonSerializer.DeserializeAsync<IEnumerable<CartModel>>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return list;
        }
        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public async Task<IActionResult> AddToCart(int id)
        {
            Cart cart = new Cart
            {
                ProductId = id,
                UserId = int.Parse(User.Claims.Where(claim => claim.Type == "UserId").FirstOrDefault().Value),
                CartId = new System.Guid()
            };
            using HttpClient client = await _httpClientHelper.GetHttpClientAsync();
            using HttpResponseMessage response = await client.PostAsync(_httpClientHelper.GetApiUrl("AddToCart"), cart, new JsonMediaTypeFormatter());
            string responseBody = response.Content.ReadAsStringAsync().Result;
            if (response.StatusCode == HttpStatusCode.OK && responseBody == "1")
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
        [Authorize(Roles = "User,Admin")]
        [HttpDelete]
        public async Task<string> RemoveItemFromCart(int Id)
        {
            using HttpClient client = await _httpClientHelper.GetHttpClientAsync();
            using HttpResponseMessage response = await client.DeleteAsync(_httpClientHelper.GetApiUrl("RemoveItemFromCart") + Id);
            string responseBody = response.Content.ReadAsStringAsync().Result;
            if (response.StatusCode == HttpStatusCode.OK && responseBody == "1")
            {
                return "Success";
            }
            else
            {
                return string.Empty;
            }
        }
        [NonAction]
        public static int GetCountofItemsinCart(IEnumerable<CartModel> CartItems, out double? totalprice)
        {
            int Count = 0;
            double? price = 0;
            foreach (CartModel item in CartItems)
            {
                price += item.Price;
                Count++;
            }
            totalprice = price;
            return Count;
        }
    }
}