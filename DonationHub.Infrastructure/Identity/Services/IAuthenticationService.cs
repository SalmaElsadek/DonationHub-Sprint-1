using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DonationHub.Infrastructure.Identity.DTOs;

namespace DonationHub.Infrastructure.Identity.Services
{
    public interface IAuthenticationService
    {
        Task<AuthDTO>RegisterAsync(RegisterDTO model);
        Task<AuthDTO> LoginAsync(LoginDTO model);
        Task<AuthDTO>RefreshTokenAsync(RefreshTokenDTO model);
        Task<bool> RevokeTokenAsync(string token);
        Task<UserProfileDTO> GetUserProfileAsync(Guid userId);
        Task<bool>CreateStaffAsync(StaffDTO model);
    }
}
