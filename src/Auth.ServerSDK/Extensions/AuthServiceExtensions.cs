using Auth.ServerSDK;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AuthServiceExtensions
    {

        public static IServiceCollection AddAuthServices(this IServiceCollection services, AuthConfigs configs)
        {
            services.AddHttpClient<AuthSDK>(c =>
            {
                c.BaseAddress = new(configs.Endpoint);
                c.DefaultRequestHeaders.Add("header_auth", configs.HeaderKey);
            });

            return services;
        }
    }
}
