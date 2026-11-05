using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonationHub.Application.Options
{
    public class JwtOptions
    {
        public const string SectionName = "Jwt";
        [Required]
        [MinLength(32)]
        public string SecretKey { get; set; } = default!;
        [Required]
        public string Issuer { get; set; } = default!;
        [Required]
        public string Audience { get; set; } = default!;
        [Range(1,24*60)]
        public int ExpiryMinutes { get; set; } = 60;
        [Range(1,365)]
        public int RefreshTokenExpiryDays { get; set; }

    }
}
