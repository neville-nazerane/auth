using Microsoft.AspNetCore.Identity;

namespace Auth.ServerLogic.Entities
{
    public class User : IdentityUser<int>
    {

        public bool IsPermanent { get; set; }

        public DateTime CreatedOn { get; set; }

        public IEnumerable<RefreshToken>? RefreshTokens { get; set; }

        public User()
        {
            CreatedOn = DateTime.UtcNow;
        }

    }
}
