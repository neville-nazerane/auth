using Auth.WebAPI.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Auth.ApiConsumer
{
    public class AuthApiHandler(AuthService service) : HttpClientHandler
    {

        private readonly AuthService _service = service;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                                     CancellationToken cancellationToken)
        {
            var token = await _service.GetJwtAsync(cancellationToken: cancellationToken);

            if (token is not null)
                request.Headers.Authorization = new("Bearer", token);

            var res = await base.SendAsync(request, cancellationToken);

            if (res.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {

            }

            if (res.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var errors = await res.Content.ReadFromJsonAsync<IEnumerable<string>>(cancellationToken: cancellationToken) ?? [];
                throw new BadRequestException(errors);
            }
            return res;
        }

    }
}
