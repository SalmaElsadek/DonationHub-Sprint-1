using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonationHub.Infrastructure.Identity.DTOs
{
    public class RefreshTokenDTO
    {
        public string RefreshToken { get; set; } = default!;
    }
}
