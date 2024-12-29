using Auth.ServerLogic.Services;
using Auth.WebAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Background.Workers
{
    public class UserCleanupWorker(IServiceProvider serviceProvider) : BackgroundService
    {

        private readonly IServiceProvider _serviceProvider = serviceProvider;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                await using var scope = _serviceProvider.CreateAsyncScope();
                var db = scope.ServiceProvider.GetService<AppDbContext>();

                db.Users.Where(u => !u.IsPermanent && );

                await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
            }
        }
    }
}
