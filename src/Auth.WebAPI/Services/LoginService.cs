using Auth.Models;
using Auth.ServerLogic.Entities;
using Auth.ServerLogic.Services;
using Auth.WebAPI.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Auth.WebAPI.Services
{
    public class LoginService(SignInManager<User> manager,
                              AppDbContext context,
                              TokenGenerator tokenGenerator)
    {

        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        private readonly SignInManager<User> _manager = manager;
        private readonly AppDbContext _context = context;
        private readonly TokenGenerator _tokenGenerator = tokenGenerator;


        public async Task<TokenResponse> GetJwtForLoginAsync(LoginModel model,
                                                             CancellationToken cancellationToken = default)
        {
            var user = await context.Users.SingleOrDefaultAsync(u => u.UserName == model.UserName, cancellationToken: cancellationToken);

            if (user?.UserName is not null)
            {
                var valid = await _manager.CheckPasswordSignInAsync(user, model.Password ?? "", false);
                if (valid.Succeeded)
                    return await GenerateJwtTokenAsync(user, cancellationToken);
            }
            throw new BadRequestException(["Invalid Login"]);
        }

        public async Task<TokenResponse> GetJwtForRefreshAsync(string refreshToken,
                                                               CancellationToken cancellationToken = default)
        {
            var user = await _context.RefreshTokens
                                     .Where(r => r.Token == refreshToken)
                                     .Select(r => r.User)
                                     .SingleOrDefaultAsync(cancellationToken: cancellationToken);
            if (user is null)
                throw new BadRequestException("Invalid token");

            return await GenerateJwtTokenAsync(user, cancellationToken);
        }

        private async Task<TokenResponse> GenerateJwtTokenAsync(User user,
                                                                CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(user.UserName);

            var refreshToken = GenerateRefreshToken();

            await _context.RefreshTokens.AddAsync(new RefreshToken
            {
                Token = refreshToken,
                ExpiresOn = DateTime.UtcNow.AddMonths(3),
                UserId = user.Id
            }, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var token = _tokenGenerator.GenerateToken([
                new(ClaimTypes.Name, user.UserName),
                        new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    ], out var expiersIn);

            return new()
            {
                AccessToken = token,
                RefreshToken = refreshToken,
                ExpiresIn = expiersIn
            };
        }

        


        //public async Task<string> GenerateJwtAsync(LoginModel model,
        //                                           User? user)
        //{
        //    if (user?.UserName is not null)
        //    {
        //        var valid = await manager.CheckPasswordSignInAsync(user, model.Password ?? "", false);

        //        var refreshToken = GenerateRefreshToken();


        //        var refreshEntity = new RefreshToken
        //        {
        //            Token = refreshToken,
        //            ExpiresOn = DateTime.UtcNow.AddMonths(3),
        //            UserId = user.Id
        //        };

        //        if (valid.Succeeded)
        //        {

        //            var token = tokenGenerator.GenerateToken([
        //                new(ClaimTypes.Name, user.UserName),
        //                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
        //            ]);

        //            return token;
        //        }
        //    }
        //    throw new BadRequestException(["Invalid Login"]);
        //}

        static string GenerateRefreshToken()
            => new([.. Enumerable.Range(0, 20).Select(_ => chars[RandomNumberGenerator.GetInt32(chars.Length)])]);



    }

}
