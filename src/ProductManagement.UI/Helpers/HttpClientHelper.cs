using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProductManagement.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProductManagement.UI.Helpers
{
    public class HttpClientHelper : ControllerBase,IHttpClientHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public HttpClientHelper(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactorry,IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactorry;
            _configuration = configuration;
        }
        public string GetApiUrl(string key)
        {
            return _configuration.GetSection("ApiUrls:" + key).Value.ToString();
        }
        public async Task<HttpClient> GetHttpClientAsync()
        {
            HttpClient httpclient = _httpClientFactory.CreateClient("PMApis");
            httpclient.DefaultRequestHeaders.Add("Authorization", "Bearer " + await _httpContextAccessor.HttpContext.GetTokenAsync("Token"));
            return httpclient;
        }
    }
}
