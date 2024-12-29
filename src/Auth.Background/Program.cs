using Auth.Background;
using Auth.Background.Workers;
using Auth.ServerLogic.Utils;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<UserCleanupWorker>();

builder.Services.AddAllServices(builder.Configuration);

var host = builder.Build();
 
await host.RunAsync();
