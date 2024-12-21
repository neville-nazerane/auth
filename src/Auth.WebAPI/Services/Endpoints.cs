using Auth.Models;
using Auth.WebAPI.Entities;
using Auth.WebAPI.Exceptions;
using Microsoft.AspNetCore.Identity;
using System.Collections.Immutable;

namespace Auth.WebAPI.Services
{
    public static class Endpoints
    {

        public static void MapAllEndpoints(this IEndpointRouteBuilder endpoints)
        {

            endpoints.MapPost("signup", SignupAsync);

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

    }
}
