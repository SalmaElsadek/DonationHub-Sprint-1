using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using DonationHub.Domain.Entities;
using DonationHub.Infrastructure.Data;
using DonationHub.Infrastructure.Identity.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
namespace DonationHub.Infrastructure.Identity.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManger;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthenticationService(UserManager<ApplicationUser> userManger,RoleManager<IdentityRole<Guid>>roleManager
            ,AppDbContext context,IConfiguration configuration)
        {
           _userManger = userManger;
           _roleManager = roleManager;
           _context = context;
           _configuration = configuration;
        }
        public async Task<bool> CreateStaffAsync(StaffDTO model)
        {
            if (model.Role != "Admin" && model.Role != "CampaignManager")
                throw new Exception("Invalid staff role specified.");

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                IsActive = true,
                EmailConfirmed=true
            };

            var result=await _userManger.CreateAsync(user,model.Password);

            if (!result.Succeeded) return false;
            await _userManger.AddToRoleAsync(user, model.Role);
            return true;
        }

        public async Task<UserProfileDTO> GetUserProfileAsync(Guid userId)
        {
            var user=await _userManger.FindByIdAsync(userId.ToString());

            if (user == null || !user.IsActive)
            {
                throw new Exception("Auth.UserNotFound");
            }

            var roles = await _userManger.GetRolesAsync(user);

            return new UserProfileDTO
            {
                UserID = user.Id,
                Email = user.Email ?? string.Empty,
                FullName = user.FullName,
                PreferredLanguage = user.PreferredLanguage,
                Roles = roles
            };
        }

        public async Task<AuthDTO> LoginAsync(LoginDTO model)
        {
            var user=await _userManger.FindByEmailAsync(model.Email);
            if (user == null || !await _userManger.CheckPasswordAsync(user, model.Password)) {
                throw new Exception("Auth.InvalidCredentials");//41 UnAthor
            }

            if (!user.IsActive) {
                throw new Exception("Auth.AccountDeactivated"); //403 Forbidden
            }
            return await GenerateAuthResponseAsync(user);
        }

        public Task<AuthDTO> RefreshTokenAsync(RefreshTokenDTO model)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthDTO> RegisterAsync(RegisterDTO model)
        {
            var existingUser=await _userManger.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                throw new Exception("Auth.EmailAlreadyExists"); // 409 Conflict            }
            }
                var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                PreferredLanguage = string.IsNullOrEmpty(model.PreferredLanguage) ? "en" : model.PreferredLanguage,
                IsActive = true
            };

            var result=await _userManger.CreateAsync(user,model.Password);
            if (!result.Succeeded)
            {
                var error=string.Join(", ",result.Errors.Select(e=>e.Description));
                throw new InvalidOperationException(error);
            }

            await _userManger.AddToRoleAsync(user, "Donor");

            return await GenerateAuthResponseAsync(user);
        }

        public async Task<bool> RevokeTokenAsync(string token)
        {
            var tokenHash=HashToken(token);
            var refreshToken=await _context.RefreshTokens.FirstOrDefaultAsync(r=>r.TokenHash==tokenHash);

            if(refreshToken==null||!refreshToken.IsActive)return false;

            refreshToken.Revoke();
            await _context.SaveChangesAsync();
            return true;
        }

        //Helper Method يارب ما نبص فيها تاني
        private string HashToken(string token)
        {
            using var sha256=SHA256.Create(); 
            var hashByte=sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
            return Convert.ToBase64String(hashByte);
        }
        private RefreshToken GenerateRefreshToken(Guid userId)
        {
            var randomNumber = new byte[64];
            //old
            //using var generator = new RNGCryptoServiceProvider();

            using var generator = RandomNumberGenerator.Create();
            generator.GetBytes(randomNumber);

            var tokenString=Convert.ToBase64String(randomNumber);
            var tokenHash = HashToken(tokenString);
            var expiry = double.Parse(_configuration["Jwt:RefreshTokenExpiryDays"] ?? "7");


            return RefreshToken.Create(
                userId, tokenHash, DateTimeOffset.UtcNow.AddDays(expiry)
                );
        }

        private async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
        {
            var claims = new List<Claim> { 
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email??string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())            
            };

            var roles = await _userManger.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
            var creds=new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
            var minutesExpiry = double.Parse(_configuration["Jwt:ExpiryMinutes"] ?? "15");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(minutesExpiry),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }

        private async Task<AuthDTO> GenerateAuthResponseAsync(ApplicationUser user)
        {
            var refreshToken = GenerateRefreshToken(user.Id);
            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            var accessToken = await GenerateJwtTokenAsync(user);
            var roles = await _userManger.GetRolesAsync(user);
            var minutesExpiry = double.Parse(_configuration["Jwt:ExpiryMinutes"] ?? "15");

            return new AuthDTO
            {
                UserId = user.Id,
                Email = user.Email ?? string.Empty,
                FullName = user.FullName,
                AccessToken = accessToken,
                AccessTokenExpiration = DateTime.UtcNow.AddMinutes(minutesExpiry),
                RefreshToken = refreshToken.TokenHash,
                RefreshTokenExpiration = refreshToken.ExpiredAt.UtcDateTime,
                Roles = roles.ToList()
            };
        }
    }
}
