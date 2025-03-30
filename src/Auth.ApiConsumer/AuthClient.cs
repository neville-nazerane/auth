using Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Auth.ApiConsumer
{
    public class AuthClient(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;


        public async Task SignupAsync(SignupModel model, CancellationToken cancellationToken = default)
        {
            using var res = await _httpClient.PostAsJsonAsync("signup", model, cancellationToken: cancellationToken);
            res.EnsureSuccessStatusCode();
        }

        public async Task<TokenResponse?> LoginAsync(LoginModel model, CancellationToken cancellationToken = default)
        {
            using var res = await _httpClient.PostAsJsonAsync("login", model, cancellationToken: cancellationToken);
            res.EnsureSuccessStatusCode();
            return await res.Content.ReadFromJsonAsync<TokenResponse>(cancellationToken);
        }

        public Task<TokenResponse?> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
            => _httpClient.GetFromJsonAsync<TokenResponse>($"refreshToken/{refreshToken}", cancellationToken: cancellationToken);

    }
}
