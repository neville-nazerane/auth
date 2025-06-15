using Auth.ApiConsumer.Models;
using Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.ApiConsumer
{
    public class AuthService(AuthClient client, IAuthStore store)
    {
        private readonly AuthClient _client = client;
        private readonly IAuthStore _store = store;

        private TokenData? tokenResponse;

        public async ValueTask<string?> GetJwtAsync(bool forceUpdateFromStore = false,
                                             CancellationToken cancellationToken = default)
        {
            if (forceUpdateFromStore || tokenResponse is null)
            {
                tokenResponse = await _store.GetAsync(cancellationToken);
            }

            if (tokenResponse is not null)
            {
                if (DateTime.UtcNow >= tokenResponse.ExpiresOn.AddMinutes(-1))
                    await RefreshAsync(cancellationToken);

                return tokenResponse.JwtToken;
            }

            return null;
        }

        public async Task<bool> SignupAsync(SignupModel model,
                                            bool includeLogin = true,
                                            CancellationToken cancellationToken = default)
        {
            await _client.SignupAsync(model, cancellationToken);
            var loginModel = new LoginModel
            {
                UserName = model.UserName,
                Password = model.Password,
            };
            if (includeLogin)
                return await LoginAsync(loginModel, cancellationToken);

            // assumes signup throws on fail
            return true;
        }

        public async Task<bool> LoginAsync(LoginModel model, CancellationToken cancellationToken = default)
        {
            var response = await _client.LoginAsync(model, cancellationToken);
            if (response is not null)
            {
                await SaveTokenAsync(response, cancellationToken);
                return true;
            }
            return false;
        }

        public async Task<bool> RefreshAsync(CancellationToken cancellationToken = default)
        {
            var tokenResult = await _store.GetAsync(cancellationToken);
            if (tokenResult is null) return false;
            var res = await _client.RefreshTokenAsync(tokenResult.RefreshToken, cancellationToken);
            if (res is null) return false;
            await SaveTokenAsync(res, cancellationToken);
            return true;
        }

        public Task SignOutAsync(CancellationToken cancellationToken = default)
            => _store.RemoveAsync(cancellationToken);

        async Task<TokenData> SaveTokenAsync(TokenResponse response, CancellationToken cancellationToken = default)
        {
            var res = new TokenData
            {
                JwtToken = response.AccessToken,
                RefreshToken = response.RefreshToken,
                ExpiresOn = DateTime.UtcNow.AddSeconds(response.ExpiresIn)
            };

            await _store.SetAsync(res, cancellationToken);

            return res;
        }

    }
}
