using Microsoft.IdentityModel.Tokens;

namespace Auth.WebAPI.Models
{
    public class JwtOptions
    {

        public required string Issuer { get; set; }

        public required TimeSpan ExpiresIn { get; set; }

        public required SymmetricSecurityKey SigningKey { get; set; }

    }
}
