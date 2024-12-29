using Auth.ServerLogic.Entities;
using Auth.ServerLogic.Services;
using Auth.WebAPI.Entities;
using Auth.WebAPI.Models;
using Auth.WebAPI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Auth.WebAPI.Utils
{
    public static class ServiceExtensions
    {

        public static IServiceCollection SetupJwt(this IServiceCollection services,
                                                  IConfiguration configs)
        {
            var secret = configs["secret"] ?? throw new Exception("Secret config was not found");
            var issuer = configs["issuer"] ?? throw new Exception("Issuer config was not found");

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            var options = new JwtOptions
            {
                ExpiresIn = TimeSpan.FromHours(1),
                Issuer = issuer,
                SigningKey = signingKey
            };

            services.AddSingleton(options);

            services.AddAuthentication("JWT")
                    .AddJwtBearer("JWT", o =>
                    {
                        o.ClaimsIssuer = issuer;
                        o.TokenValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = signingKey,
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true
                        };
                    });
                    
            return services;
        }

    }
}
