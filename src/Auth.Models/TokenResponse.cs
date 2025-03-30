using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Auth.Models
{
    public class TokenResponse
    {

        [JsonPropertyName("accessToken")]
        public required string AccessToken { get; set; }

        [JsonPropertyName("refreshToken")]
        public required string RefreshToken { get; set; }

        [JsonPropertyName("expiresIn")]
        public double ExpiresIn { get; set; }

    }
}
