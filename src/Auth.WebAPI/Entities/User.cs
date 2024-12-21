using Microsoft.AspNetCore.Identity;

namespace Auth.WebAPI.Entities
{
    public class User : IdentityUser
    {

        public bool IsPermanent { get; set; }


    }
}
