using Auth.WebAPI.Services;
using Auth.WebAPI.Utils;

var builder = WebApplication.CreateBuilder(args);

var configs = builder.Configuration;
var services = builder.Services;

services.SetupJwt(configs.GetSection("auth"));

services.AddDb(configs["sqlConnString"]);

var app = builder.Build();

app.MapGet("/", () => "Hello Auth!");

app.MapAllEndpoints();

await app.RunAsync();
