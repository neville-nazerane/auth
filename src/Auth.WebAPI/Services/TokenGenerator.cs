using Auth.WebAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Auth.WebAPI.Services
{
    public class TokenGenerator(JwtOptions options)
    {
        
        private readonly JwtOptions _options = options;

        public string GenerateToken(IEnumerable<Claim> claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _options.Issuer,
                Expires = DateTime.UtcNow.Add(_options.ExpiresIn),
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new(_options.SigningKey, SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


    }
}
