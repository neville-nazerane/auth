using Auth.ServerLogic.Entities;
using Auth.ServerLogic.Services;
using Auth.WebAPI.Services;
using Auth.WebAPI.Utils;
using Auth.ServerLogic.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var configs = builder.Configuration;
var services = builder.Services;

services.SetupJwt(configs.GetSection("auth"))
                    .AddAllServices(configs);

services.AddIdentity<User, IdentityRole<string>>()
                    .AddEntityFrameworkStores<AppDbContext>();

if (builder.Environment.IsDevelopment())
    services.AddOpenApi("v1");

var app = builder.Build();

app.HandleExceptions();

app.MapGet("/", () => "Hello Auth!");

app.MapAllEndpoints();

if (builder.Environment.IsDevelopment())
    app.MapOpenApi("/docs");

await app.RunAsync();




