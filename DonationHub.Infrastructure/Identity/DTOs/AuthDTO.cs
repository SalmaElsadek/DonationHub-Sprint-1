using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonationHub.Infrastructure.Identity.DTOs
{
    public class AuthDTO
    {
        public Guid UserId { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string AccessToken { get; set; } = default!;
        public DateTimeOffset AccessTokenExpiration { get; set; }
        public string RefreshToken { get; set; } = default!;
        public DateTimeOffset RefreshTokenExpiration { get; set; }
        public IList<string> Roles { get; set; } = new List<string>();
    }
}
