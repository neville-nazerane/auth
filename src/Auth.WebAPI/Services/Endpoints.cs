using Auth.Models;
using Auth.WebAPI.Entities;
using Auth.WebAPI.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

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

        public static async Task<UserModel> LoginAsync(SignInManager<User> manager,
                                            AppDbContext context,
                                            LoginModel model)
        {
            var user = await context.Users.SingleOrDefaultAsync(u => u.UserName == model.UserName);
            
            if (user?.UserName is not null)
            {
                var valid = await manager.CheckPasswordSignInAsync(user, model.Password ?? "", false);
                if (valid.Succeeded)
                {
                    var res = new UserModel
                    {
                        UserName = user.UserName,
                        Id = user.Id
                    };

                    return res;
                }
            }
            throw new BadRequestException(["Invalid Login"]);
        }

    }
}
