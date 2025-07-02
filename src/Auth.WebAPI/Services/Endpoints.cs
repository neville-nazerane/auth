using Auth.Models;
using Auth.ServerLogic.Entities;
using Auth.ServerLogic.Services;
using Auth.WebAPI.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Auth.WebAPI.Services
{
    public static class Endpoints
    {


        public static void MapAllEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("signup", SignupAsync);

            endpoints.MapPost("login", LoginAsync);
            endpoints.MapGet("refreshToken/{refreshToken}", RefreshTokenAsync);

            endpoints.MapPost("fetchValidIds", GetValidIdsAsync).RequireAuthorization();
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

        static Task<TokenResponse> LoginAsync(LoginService service,
                                             LoginModel model,
                                             CancellationToken cancellationToken = default)
            => service.GetJwtForLoginAsync(model, cancellationToken);

        static Task<TokenResponse> RefreshTokenAsync(LoginService service,
                                                            string refreshToken,
                                                            CancellationToken cancellationToken = default)
            => service.GetJwtForRefreshAsync(refreshToken, cancellationToken);

        public static async IAsyncEnumerable<int> GetValidIdsAsync(IAsyncEnumerable<int> userIds, AppDbContext context)
        {
            List<int> ids = [];

            await foreach (var userId in userIds)
            {
                ids.Add(userId);
                if (ids.Count > 9)
                {
                    var dbIds = context.Users
                                       .Where(u => ids.Contains(u.Id))
                                       .Select(s => s.Id)
                                       .AsAsyncEnumerable();

                    await foreach (var id in dbIds)
                        yield return id;

                    ids.Clear();
                }
            }
            
        }

    }
}
