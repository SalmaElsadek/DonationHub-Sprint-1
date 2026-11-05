using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonationHub.Infrastructure.Identity.DTOs
{
    public class UserProfileDTO
    {
        public Guid UserID { get; set; }
        public string Email { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string PreferredLanguage { get; set; } = "en";
        public string Password { get; set; }=default!;
        public IList<string> Roles { get; set; }=new List<string>();

    }
}
