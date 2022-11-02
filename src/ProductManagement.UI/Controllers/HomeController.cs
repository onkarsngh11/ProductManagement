using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProductManagement.Entities;
using ProductManagement.Entities.Models;
using ProductManagement.Interfaces;

namespace ProductManagement.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _host;
        private readonly CartController _cartController;
        private readonly IHttpClientHelper _httpClientHelper;
        private readonly IConfiguration _configuration;
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment hostenvironment, IHttpClientHelper httpClientHelper, CartController cartController, IConfiguration configuration)
        {
            _logger = logger;
            _host = hostenvironment;
            _httpClientHelper = httpClientHelper;
            _cartController = cartController;
            _configuration = configuration;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                int count = await GetItemsInCartFromCartController();
                if (count == 0)
                {
                    ViewBag.TotalProducts = 0;
                }
                else
                {
                    ViewBag.TotalProducts = count;
                }
                IEnumerable<ProductsModel> listOfProducts = await GetListofProducts();
                return View(listOfProducts);
            }
            else
            {
                return RedirectToAction("Login", "Auth");
            }
        }

        [NonAction]
        private async Task<int> GetItemsInCartFromCartController()
        {
            return CartController.GetCountofItemsinCart(await _cartController.GetItemsInCart(), totalprice: out _);
        }

        [NonAction]
        private async Task<ProductsModel> GetProductById(int id)
        {
            using HttpClient client = await _httpClientHelper.GetHttpClientAsync();
            using HttpResponseMessage response = await client.GetAsync(_httpClientHelper.GetApiUrl("GetProductById") + id);
            Stream responseBody = response.Content.ReadAsStreamAsync().Result;
            ProductsModel product = await JsonSerializer.DeserializeAsync<ProductsModel>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return product;
        }
        [NonAction]
        private async Task<IEnumerable<ProductsModel>> GetListofProducts()
        {
            using HttpClient client = await _httpClientHelper.GetHttpClientAsync();
            using HttpResponseMessage response = await client.GetAsync(_httpClientHelper.GetApiUrl("GetListOfAllProducts"));
            Stream responseBody = response.Content.ReadAsStreamAsync().Result;
            if (responseBody.Length != 0)
            {
                IEnumerable<ProductsModel> list = await JsonSerializer.DeserializeAsync<IEnumerable<ProductsModel>>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return list;
            }

            return null;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult AddProducts()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddProducts(ProductsModel productsModel)
        {
            string uniqueFileName = ProcessUploadedFile(productsModel);
            productsModel.ImageUrl = uniqueFileName;
            productsModel.SalePrice = GetSalePrice(productsModel.Price);
            productsModel.Image = GetImageName(productsModel.UploadedFiles);
            using HttpClient client = await _httpClientHelper.GetHttpClientAsync();
            using HttpResponseMessage response = await client.PostAsync(_httpClientHelper.GetApiUrl("AddProduct"), productsModel, new JsonMediaTypeFormatter());
            string responseBody = response.Content.ReadAsStringAsync().Result;
            if (response.StatusCode == HttpStatusCode.OK && responseBody == "1")
            {
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }
        }

        [NonAction]
        private double? GetSalePrice(double? price)
        {
            return Math.Round(price * 1.1 ?? 0, 2);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<string> DeleteProduct(int id)
        {
            using HttpClient client = await _httpClientHelper.GetHttpClientAsync();
            using HttpResponseMessage response = await client.DeleteAsync(_httpClientHelper.GetApiUrl("DeleteProduct") + id);
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
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> UpdateProduct(int id)
        {
            ProductsModel product = await GetProductById(id);
            return View(product);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> UpdateProduct(ProductsModel productsModel)
        {
            if (productsModel.UploadedFiles != null)
            {
                string uniqueFileName = ProcessUploadedFile(productsModel);
                productsModel.ImageUrl = uniqueFileName;
                productsModel.Image = GetImageName(productsModel.UploadedFiles);
            }
            productsModel.SalePrice = GetSalePrice(productsModel.Price);
            using HttpClient client = await _httpClientHelper.GetHttpClientAsync();
            using HttpResponseMessage response = await client.PostAsync(_httpClientHelper.GetApiUrl("UpdateProduct"), productsModel, new JsonMediaTypeFormatter());
            string responseBody = response.Content.ReadAsStringAsync().Result;
            if (response.StatusCode == HttpStatusCode.OK && responseBody == "1")
            {
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }
        }
        [HttpDelete]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [NonAction]
        public string ProcessUploadedFile(ProductsModel model)
        {
            string uniqueFileName = null;
            if (model.UploadedFiles! != null)
            {
                string uploadsFolder = Path.Combine(_host.WebRootPath, "Images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.UploadedFiles.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using FileStream fs = new FileStream(filePath, FileMode.Create);
                model.UploadedFiles.CopyTo(fs);
            }
            return uniqueFileName;
        }

        [NonAction]
        public void DeleteOldUploadedImages(Products model)
        {
            if (model != null)
            {
                string uploadsFolder = Path.Combine(_host.WebRootPath, "Images");
                string filePath = Path.Combine(uploadsFolder, model.ImageUrl);
                System.IO.File.Delete(filePath);
            }
        }
        [NonAction]
        public static string GetImageName(IFormFile photo)
        {
            if (photo != null)
            {
                return Path.GetFileName(photo.FileName);
            }
            return string.Empty;
        }
    }
}
