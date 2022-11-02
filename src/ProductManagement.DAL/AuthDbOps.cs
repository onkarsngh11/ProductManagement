using Microsoft.EntityFrameworkCore;
using ProductManagement.Entities;
using ProductManagement.Entities.Models;
using ProductManagement.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagement.DAL
{
    public class AuthDbOps : IAuthDbOps
    {
        private readonly ProductManagementDbContext _context;

        public AuthDbOps(ProductManagementDbContext dbContext)
        {
            _context = dbContext;
        }
        public async Task<User> AuthenticateUser(LoginModel loginModel)
        {
            User user = await _context.Users.Where(u => u.UserName == loginModel.UserName && u.Password == loginModel.Password).FirstOrDefaultAsync();
            return user;
        }

        public async Task<int> Register(RegisterModel registerModel)
        {
            User user = new User
            {
                UserId = Guid.NewGuid(),
                Email = registerModel.Email,
                UserRole = "Admin",
                UserName = registerModel.UserName,
                Password = registerModel.Password,
                FullName = registerModel.FullName
            };
            _context.Users.Add(user);
            return await _context.SaveChangesAsync();
        }

    }
}
