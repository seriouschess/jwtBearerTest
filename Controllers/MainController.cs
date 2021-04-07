using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using jwtBearerTest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace jwtBearerTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MainController : ControllerBase
    {
        private IConfiguration _config;
        public MainController(IConfiguration config){
            this._config = config;
        }

        [HttpGet]
        [Authorize]
        [Route("authenticate")]
        public string Authenticate()
        {
            return "success";
        }

        [HttpPost]
        [Route("token/get")]
        public IActionResult GetToken( [FromBody] LoginModel loginModel ){
            IActionResult response = Unauthorized();

            var tokenString = GenerateJWTToken(loginModel);
                response = Ok(new
                {
                    token = tokenString,
                    userDetails = loginModel.ToString()
                });
            return response;
        }

        private string GenerateJWTToken(LoginModel Info)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes( _config["Jwt:PrivateKey"] ));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("Email", Info.email),
                new Claim("Password", Info.password)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddSeconds(int.Parse(_config["Jwt:LifetimeInSeconds"])),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
