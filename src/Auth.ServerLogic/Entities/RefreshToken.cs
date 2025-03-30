using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.ServerLogic.Entities
{
    public class RefreshToken
    {

        public int Id { get; set; }

        [MaxLength(40)]
        public required string Token { get; set; }

        [Required]
        public DateTime? CreatedOn { get; set; }

        [Required]
        public DateTime? ExpiresOn { get; set; }

        public DateTime? UsedOn { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }


        public RefreshToken()
        {
            CreatedOn = DateTime.UtcNow;
        }

    }
}
