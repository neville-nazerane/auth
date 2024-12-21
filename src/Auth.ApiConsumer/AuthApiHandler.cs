using Auth.WebAPI.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Auth.ApiConsumer
{
    public class AuthApiHandler : HttpClientHandler
    {

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                                     CancellationToken cancellationToken)
        {
            var res = await base.SendAsync(request, cancellationToken);
            if (res.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var errors = await res.Content.ReadFromJsonAsync<IEnumerable<string>>(cancellationToken: cancellationToken) ?? [];
                throw new BadRequestException(errors);
            }
            return res;
        }

    }
}
