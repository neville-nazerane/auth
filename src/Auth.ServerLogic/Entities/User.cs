using Microsoft.AspNetCore.Identity;

namespace Auth.ServerLogic.Entities
{
    public class User : IdentityUser
    {

        public bool IsPermanent { get; set; }

        public DateTime CreatedOn { get; set; }

        public User()
        {
            CreatedOn = DateTime.UtcNow;
        }

    }
}
