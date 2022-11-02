using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Entities.Models;
using ProductManagement.Interfaces;

namespace ProductManagement.UI.Controllers
{
    [AllowAnonymous]
    public class AuthController : BaseController
    {
        private readonly IHttpClientHelper _httpClientHelper;
        public AuthController(IHttpClientHelper httpClientHelper)
        {
            _httpClientHelper = httpClientHelper;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            loginModel.Password = EncodePasswordToBase64(loginModel.Password);

            using HttpClient client = await _httpClientHelper.GetHttpClientAsync();
            using HttpResponseMessage response = await client.PostAsync(_httpClientHelper.GetApiUrl("Login"), loginModel, new JsonMediaTypeFormatter());
            CookieModel cookie = response.Content.ReadAsAsync<CookieModel>().Result;
            if (response.StatusCode == HttpStatusCode.OK && cookie != null)
            {
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                JwtSecurityToken Token = handler.ReadJwtToken(cookie.Token);
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(Token.Claims, CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties authProperties = new AuthenticationProperties();
                List<AuthenticationToken> tokens = new List<AuthenticationToken>
                    {
                        new AuthenticationToken { Name = "Token", Value = cookie.Token }
                    };
                authProperties.StoreTokens(tokens);
                authProperties.IsPersistent = true;

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return NotFound();
            }
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            registerModel.Password = EncodePasswordToBase64(registerModel.Password);

            using HttpClient client = await _httpClientHelper.GetHttpClientAsync();
            using HttpResponseMessage response = await client.PostAsync(_httpClientHelper.GetApiUrl("Register"), registerModel, new JsonMediaTypeFormatter());
            string responseBody = response.Content.ReadAsStringAsync().Result;
            if (response.StatusCode == HttpStatusCode.OK && responseBody == "1")
            {
                LoginModel loginModel = new LoginModel
                {
                    UserName = registerModel.UserName,
                    Password = registerModel.Password
                };
                return RedirectToAction("Login", loginModel);
            }
            else
            {
                return NotFound();
            }
        }
        [NonAction]
        public static string EncodePasswordToBase64(string password)
        {
            byte[] encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
            string encodedData = Convert.ToBase64String(encData_byte);
            return encodedData;
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}