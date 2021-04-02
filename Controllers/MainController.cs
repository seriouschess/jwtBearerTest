using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace jwtBearerTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MainController : ControllerBase
    {
        private readonly JwtGenerator jwtGenerator;
        public MainController(IConfiguration config){
            jwtGenerator = new JwtGenerator(config["Jwt:PrivateKey"], config["Jwt:LifetimeInSeconds"]);
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

           JwtGenerator token = jwtGenerator
            .AddClaim(new Claim("Email", loginModel.email))
            .AddClaim(new Claim("Password", loginModel.password));

            return Ok(new {
                Token = token.GetToken(),
                ExpirationInUnixTime = token.GetTokenExpirationInUnixTime
            });
       }
    }

    public class JwtGenerator{
        private readonly JwtHeader jwtHeader;
        private readonly List<Claim> jwtClaims;
        private readonly DateTime jwtDate;
        private readonly int tokenLifetimeInSeconds;

        public JwtGenerator(string secret_key, string expiration_time_string_seconds){
            var credentials = new SigningCredentials(
                key: new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(secret_key)
                ),
                algorithm: SecurityAlgorithms.HmacSha256
            );

            jwtHeader = new JwtHeader(credentials);
            jwtClaims = new List<Claim>();
            jwtDate = DateTime.UtcNow;
            tokenLifetimeInSeconds = int.Parse(expiration_time_string_seconds);
        }

        public JwtGenerator AddClaim(Claim claim){
            jwtClaims.Add(claim);
            return this;
        }

        public long GetTokenExpirationInUnixTime => new DateTimeOffset(
            jwtDate.AddSeconds(tokenLifetimeInSeconds)
        ).ToUnixTimeMilliseconds();

        public string GetToken() {
            var jwt = new JwtSecurityToken(
                jwtHeader,
                new JwtPayload(
                    audience: "localhost:5000",
                    issuer:"localhost:5000",
                    notBefore: jwtDate,
                    expires: jwtDate.AddSeconds(tokenLifetimeInSeconds),
                    claims: jwtClaims
                )
            );

            IdentityModelEventSource.ShowPII = true;

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }

    public class LoginModel
    {
        public string email {get;set;}
        public string password {get;set;}
    }
}
