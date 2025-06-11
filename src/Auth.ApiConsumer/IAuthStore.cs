using Auth.ApiConsumer.Models;
using Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.ApiConsumer
{
    public interface IAuthStore
    {

        public Task SetAsync(TokenData tokenResponse, CancellationToken cancellationToken = default);

        public Task<TokenData?> GetAsync(CancellationToken cancellationToken = default);

    }
}
