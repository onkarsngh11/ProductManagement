using Microsoft.EntityFrameworkCore;
using ProductManagement.Entities;
using ProductManagement.Entities.Models;
using ProductManagement.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagement.DAL
{
    public class AuthDbOps : IAuthDbOps
    {
        public async Task<User> AuthenticateUser(LoginModel loginModel)
        {
            using ProductManagementDbContext _context = new ProductManagementDbContext();
            User user = await _context.Users.Where(u => u.UserName == loginModel.UserName && u.Password == loginModel.Password).FirstOrDefaultAsync();
            return user;
        }

        public async Task<int> Register(RegisterModel registerModel)
        {
            using ProductManagementDbContext _context = new ProductManagementDbContext();
            User user = new User
            {
                Email = registerModel.Email,
                UserRole = "User",
                UserName = registerModel.UserName,
                Password = registerModel.Password,
                FullName = registerModel.FullName
            };
            _context.Users.Add(user);
            return await _context.SaveChangesAsync();
        }

    }
}
