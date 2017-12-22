using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
using GestHome.Dtos;
using System.Threading.Tasks;
using GestHome.Model;

namespace GestHome.Controllers
{
    [Produces("application/json")]
    [Route("api/Authenticate")]
    public class AuthenticateController : Controller
    {
        private readonly GestHomeContext _context;
        private readonly IConfiguration _config;

        public AuthenticateController(GestHomeContext context, IConfiguration configuration)
        {
            _context = context;
            _config = configuration;
        }

        // POST: api/Users/Login
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<Usuarios>Authenticate([FromBody] dtoLogin login)
        {
            var user = _context.Usuarios.Where(u => u.Email == login.email && u.Password == login.pwd).FirstOrDefault();

            //Una vez logeado, se añade el token
            if (user != null)
            {
                var claims = new[]
                {
                  new Claim(JwtRegisteredClaimNames.Sub,login.email),
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWTSettings:SecretKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_config["JWTSettings:Issuer"],
                  _config["JWTSettings:Audience"],
                  claims,
                  expires: DateTime.Now.AddDays(60),
                  signingCredentials: creds);

                user.Token = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
            }

            return user;
        }

    }
}