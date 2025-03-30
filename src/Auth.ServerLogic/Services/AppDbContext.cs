using Auth.ServerLogic.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth.ServerLogic.Services
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User, IdentityRole<int>, int>(options)
    {

        public DbSet<RefreshToken> RefreshTokens { get; set; }

    }
}
