using Auth.WebAPI.Services;
using Auth.WebAPI.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var configs = builder.Configuration;
var services = builder.Services;

services.SetupJwt(configs.GetSection("auth"))
        .AddDbWithIdentity(configs["sqlConnString"])
        
        .AddOpenApi("v1");

var app = builder.Build();

app.HandleExceptions();

app.MapGet("/", () => "Hello Auth!");

app.MapAllEndpoints();

app.MapOpenApi("/docs");

await app.RunAsync();




