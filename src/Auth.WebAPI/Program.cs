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

services.AddAuthentication()
        .AddScheme<HeaderAuthenticationptions, HeaderAuthenticationHandler>(HeaderAuthenticationHandler.SCHEME_NAME, o =>
        {
            o.Key = configs["headerKey"];
        });
services.AddAuthorization(o => o.AddPolicy(HeaderAuthenticationHandler.SCHEME_NAME,
                                c => c.AddAuthenticationSchemes(HeaderAuthenticationHandler.SCHEME_NAME)));

services.AddIdentity<User, IdentityRole<int>>()
                    .AddEntityFrameworkStores<AppDbContext>();

services.AddTransient<LoginService>();

if (builder.Environment.IsDevelopment())
    services.AddOpenApi("v1");

builder.Services.AddCors(options =>
{
    var cors = configs["cors"];
    if (cors is not null)
    {
        var urls = cors.Split(',');
        options.AddDefaultPolicy(policy =>
        {
            policy.WithOrigins(urls)
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    }

});

var app = builder.Build();

app.HandleExceptions();

app.UseCors();

app.UseAuthentication().UseAuthorization();

app.MapGet("/", () => "Hello Auth!");

app.MapAllEndpoints();

if (builder.Environment.IsDevelopment())
    app.MapOpenApi("/docs");

await app.RunAsync();




