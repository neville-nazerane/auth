using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Auth.WebAPI.Services
{
    public class HeaderAuthenticationHandler(IOptionsMonitor<HeaderAuthenticatinOptions> options,
                                             ILoggerFactory logger,
                                             UrlEncoder encoder)
        : AuthenticationHandler<HeaderAuthenticatinOptions>(options, logger, encoder)
    {

        public const string SCHEME_NAME = "header_auth";

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var headers = Context.Request.Headers[SCHEME_NAME];
            if (headers.Count == 1 && Options.Key is not null && Options.Key == headers.First())
            {
                var identity = new ClaimsIdentity([new Claim(ClaimTypes.Name, "batman")]);
                return Task.FromResult(AuthenticateResult.Success(new(new(identity), SCHEME_NAME)));
            }

            return Task.FromResult(AuthenticateResult.Fail("No or invalid header"));
        }
    }

    public class HeaderAuthenticatinOptions : AuthenticationSchemeOptions
    {

        public string? Key { get; set; }

    }
}
