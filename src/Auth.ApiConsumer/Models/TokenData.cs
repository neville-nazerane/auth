using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.ApiConsumer.Models
{
    public class TokenData
    {

        public required string JwtToken { get; set; }

        public required string RefreshToken { get; set; }

        public required DateTime ExpiresOn { get; set; }

    }
}
