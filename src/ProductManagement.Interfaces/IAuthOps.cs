using ProductManagement.Entities;
using ProductManagement.Entities.Models;
using System.Threading.Tasks;

namespace ProductManagement.Interfaces
{
    public interface IAuthOps
    {
        Task<int> Register(RegisterModel registerModel);
        Task<User> AuthenticateUser(LoginModel loginModel);
    }
}
