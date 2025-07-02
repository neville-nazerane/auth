using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace Auth.WebAPI.Services
{
    public class HeaderAuthenticationHandler(IOptionsMonitor<HeaderAuthenticationptions> options,
                                             ILoggerFactory logger,
                                             UrlEncoder encoder)
        : AuthenticationHandler<HeaderAuthenticationptions>(options, logger, encoder)
    {

        public const string SCHEME_NAME = "header_auth";

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var headers = Context.Request.Headers[SCHEME_NAME];
            if (headers.Count == 1 && Options.Key is not null && Options.Key == headers.First())
                return Task.FromResult(AuthenticateResult.Success(new(new(), SCHEME_NAME)));
            
            return Task.FromResult(AuthenticateResult.Fail("No or invalid header"));
        }
    }

    public class HeaderAuthenticationptions : AuthenticationSchemeOptions
    {

        public string? Key { get; set; }

    }
}
