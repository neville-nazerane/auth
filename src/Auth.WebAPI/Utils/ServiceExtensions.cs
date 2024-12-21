using Auth.WebAPI.Models;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Auth.WebAPI.Utils
{
    public static class ServiceExtensions
    {

        public static void SetupJwt(this IServiceCollection services,
                                    IConfiguration configs)
        {
            var secret = configs["secret"] ?? throw new Exception("Secret config was not found");
            var issuer = configs["issuer"] ?? throw new Exception("Issuer config was not found");

            var signingKey = new SigningCredentials(
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                            SecurityAlgorithms.HmacSha256Signature);

            var options = new JwtOptions
            {
                ExpiresIn = TimeSpan.FromHours(1),
                Issuer = issuer,
                SigningKey = signingKey
            };
            
        }

        public static IServiceCollection AddAllServices(this IServiceCollection services,
                                                        IConfiguration configs)
        {

            return services;
        }

    }
}
