using Auth.ServerLogic.Entities;
using Auth.ServerLogic.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.ServerLogic.Utils
{
    public static class ServiceExtensions
    {

        public static IServiceCollection AddAllServices(this IServiceCollection services,
                                                 IConfiguration configs)
        {

            string? connectionString = configs["sqlConnString"];
            ArgumentNullException.ThrowIfNull(connectionString);

            services.AddDbContext<AppDbContext>(c => c.UseSqlServer(connectionString));

            return services;
        }
    }
}
