using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Auth.ServerSDK
{
    public class AuthSDK(HttpClient client)
    {
        private readonly HttpClient _client = client;

        public async IAsyncEnumerable<int> GetValidIdsAsync(IEnumerable<int> ids, 
                                                            [EnumeratorCancellation]CancellationToken cancellationToken = default)
        {
            using var res = await _client.PostAsJsonAsync("fetchValidIds", ids, cancellationToken);
            res.EnsureSuccessStatusCode();
            var results = res.Content.ReadFromJsonAsAsyncEnumerable<int>(cancellationToken);
            await foreach (var result in results)
                yield return result;
        }
    }
}
