using ProductManagement.Entities;
using ProductManagement.Entities.Models;
using System.Threading.Tasks;

namespace ProductManagement.Interfaces
{
    public interface IAuthDbOps
    {
        Task<int> Register(RegisterModel registerModel);
        Task<User> AuthenticateUser(LoginModel loginModel);
    }
}
