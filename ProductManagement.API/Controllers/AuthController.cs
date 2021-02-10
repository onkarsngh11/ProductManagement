using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ProductManagement.Entities;
using ProductManagement.Entities.Models;
using ProductManagement.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthentication.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public IAuthOps _authOps;
        private readonly IConfiguration _config;
        public AuthController(IConfiguration config, IAuthOps authOps)
        {
            _config = config;
            _authOps = authOps;
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginModel login)
        {
            IActionResult response = Unauthorized();
            User authenticatedUser = await AuthenticateUser(login);
            if (authenticatedUser != null)
            {
                string tokenString = GenerateJWTToken(authenticatedUser);
                response = Ok(new
                {
                    Token = tokenString
                });
            }
            return response;
        }
        private async Task<User> AuthenticateUser(LoginModel loginModel)
        {
            return await _authOps.AuthenticateUser(loginModel);
        }
        string GenerateJWTToken(User user)
        {

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            Claim[] claims = new[]
            {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(ClaimTypes.Role, user.UserRole),
                    new Claim("UserId",Convert.ToString(user.UserId)),
                    new Claim("UserName",user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
            JwtSecurityToken token = new JwtSecurityToken
            (
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(12),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [HttpPost]
        [Route("Register")]
        public async Task<int> Register(RegisterModel registerModel)
        {
            return await _authOps.Register(registerModel);
        }
    }
}
