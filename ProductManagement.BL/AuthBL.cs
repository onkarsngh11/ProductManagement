using ProductManagement.Entities;
using ProductManagement.Entities.Models;
using ProductManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.BL
{
    public class AuthBL : IAuthOps
    {
        public IAuthDbOps _authDbOps;
        public AuthBL(IAuthDbOps authDbOps)
        {
            _authDbOps = authDbOps;
        }

        public Task<User> AuthenticateUser(LoginModel loginModel)
        {
            return _authDbOps.AuthenticateUser(loginModel);
        }

        public async Task<int> Register(RegisterModel registerModel)
        {
            return await _authDbOps.Register(registerModel);
        }
    }
}
