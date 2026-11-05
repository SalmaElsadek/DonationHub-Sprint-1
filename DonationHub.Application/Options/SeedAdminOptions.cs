using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonationHub.Application.Options
{
    public class SeedAdminOptions
    {
        public const string SectionName = "SeedAdmin";
        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;
        [Required]
        [MinLength(6, ErrorMessage = "Seed Admin password must be at least 6 characters.")]

        public string Password { get; set; } = default!;
        [Required]
        public string FullName { get; set; } = default!;
    }
}
