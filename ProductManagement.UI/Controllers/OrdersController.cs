using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Entities;
using ProductManagement.Entities.Models;
using ProductManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProductManagement.UI.Controllers
{
    public class OrdersController : BaseController
    {
        private readonly IHttpClientHelper _httpClientHelper;
        public OrdersController(IHttpClientHelper httpClientHelper)
        {
            _httpClientHelper = httpClientHelper;
        }
        // GET: Orders
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult> Index()
        {
            IEnumerable<OrdersModel> listOfOrders = await GetlistOfOrders(Convert.ToInt32(User.Claims.Where(o=>o.Type=="UserId").FirstOrDefault().Value));
            return View(listOfOrders);
        }
        [NonAction]
        public async Task<IEnumerable<OrdersModel>> GetlistOfOrders(int userId)
        {
            using HttpClient client = await _httpClientHelper.GetHttpClientAsync();
            using HttpResponseMessage response = await client.GetAsync(_httpClientHelper.GetApiUrl("GetlistOfOrders") + userId);
            System.IO.Stream responseBody = response.Content.ReadAsStreamAsync().Result;
            IEnumerable<OrdersModel> list = await JsonSerializer.DeserializeAsync<IEnumerable<OrdersModel>>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return list;
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public async Task<string> PlaceOrder(string[] IDs)
        {
            PlaceOrderModel placeOrder = new PlaceOrderModel
            {
                UserId = Convert.ToInt32(Convert.ToInt32(User.Claims.Where(o => o.Type == "UserId").FirstOrDefault().Value)), 
                IDs=IDs
            };
            using HttpClient client = await _httpClientHelper.GetHttpClientAsync();
            using HttpResponseMessage response = await client.PostAsync(_httpClientHelper.GetApiUrl("PlaceOrder"), placeOrder, new JsonMediaTypeFormatter());
            string responseBody = response.Content.ReadAsStringAsync().Result;
            if (response.StatusCode == HttpStatusCode.OK && Convert.ToInt32(responseBody) > 0)
            {
                return "Success";
            }
            else
            {
                return "Fail";
            }
        }
    }
}