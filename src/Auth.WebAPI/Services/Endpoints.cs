using Auth.Models;
using Auth.ServerLogic.Entities;
using Auth.ServerLogic.Services;
using Auth.WebAPI.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Auth.WebAPI.Services
{
    public static class Endpoints
    {


        public static void MapAllEndpoints(this IEndpointRouteBuilder endpoints)
        {

            endpoints.MapPost("signup", SignupAsync);

            endpoints.MapPost("login", LoginAsync);
        }




        static async Task SignupAsync(SignInManager<User> manager,
                                      SignupModel model)
        {
            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
            };
            var res = await manager.UserManager.CreateAsync(user, model.Password ?? string.Empty);

            if (!res.Succeeded)
            {
                var errors = res.Errors.Select(e => e.Description).ToImmutableArray();
                throw new BadRequestException(errors);
            }
        }

        static async Task<string> LoginAsync(SignInManager<User> manager,
                                                        AppDbContext context,
                                                        TokenGenerator tokenGenerator,
                                                        LoginModel model)
        {

            return await GenerateJwtAsync(manager, tokenGenerator, model, user);
        }

    }
}
