using Auth.WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(o =>
{
    
}).AddBearerToken(o =>
{
    
});

var app = builder.Build();

app.MapGet("/", () => "Hello Auth!");

app.MapAllEndpoints();

await app.RunAsync();
