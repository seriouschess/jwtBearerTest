using System.Text;  
using Microsoft.IdentityModel.Tokens;  
using Microsoft.Extensions.Configuration;  
using Microsoft.Extensions.DependencyInjection;  
using Microsoft.AspNetCore.Authentication.JwtBearer;  
  

namespace jwtBearerTest.Middleware
{
    public static class JwtAuthenticationMiddleware
    {
        public static IServiceCollection AddTokenAuthentication(this IServiceCollection services, IConfiguration config)  
        {  
            var secret = config.GetSection("Jwt").GetSection("PrivateKey").Value;

            System.Console.WriteLine($"Secret Key: {secret}");
  
            var key = Encoding.ASCII.GetBytes(secret);  
            services.AddAuthentication(x =>  
            {  
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;  
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;  
            })  
            .AddJwtBearer(x =>  
            {  
                x.TokenValidationParameters = new TokenValidationParameters  
                {  
                    IssuerSigningKey = new SymmetricSecurityKey(key),  
                    ValidateIssuer = false,  
                    ValidateAudience = false,  
                    ValidIssuer = "localhost:5000",  
                    ValidAudience = "localhost:5000"  
                };  
            });  
  
            return services;  
        }
    }
} 