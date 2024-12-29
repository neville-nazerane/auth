using Auth.Background;
using Auth.Background.Workers;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<UserCleanupWorker>();

var host = builder.Build();
 
await host.RunAsync();
