using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonationHub.Infrastructure.Identity.DTOs
{
    public class StaffDTO
    {
        public string Email { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Role { get; set; } = default!;
    }
}
