using System.Net.Http;
using System.Threading.Tasks;

namespace ProductManagement.Interfaces
{
    public interface IHttpClientHelper
    {
        Task<HttpClient> GetHttpClientAsync();
        string GetApiUrl(string key);
    }
}
