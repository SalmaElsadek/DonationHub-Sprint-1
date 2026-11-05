using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DonationHub.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace DonationHub.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FullName { get; set; } = default!;
        public string PreferredLanguage { get; set; } = "en";
        public bool IsActive { get; set; } = true;
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    }
}
