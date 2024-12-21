using Auth.WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello Auth!");

app.MapAllEndpoints();

await app.RunAsync();
