using DonationHub.Infrastructure.Identity.DTOs;
using DonationHub.Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DonationHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _service;

        public AuthController(IAuthenticationService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthDTO>> Register([FromBody] RegisterDTO model)
        {
            try
            {
                var result = await _service.RegisterAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                if(ex.Message== "Auth.EmailAlreadyExist")
                    return Conflict(ex.Message);

                return BadRequest(ex.Message);
            }
        }
        [HttpPost("login")]
        public async Task<ActionResult<AuthDTO>> Login([FromBody] LoginDTO model)
        {
            try
            {
                var result = await _service.LoginAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Auth.InvalidCredentials")
                    return Unauthorized(ex.Message);

                if (ex.Message == "Auth.AccountDeactivated")
                    return StatusCode(StatusCodes.Status403Forbidden, ex.Message);

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("refresh_token")]
        public async Task<ActionResult<AuthDTO>> RefreshToken([FromBody] RefreshTokenDTO model)
        {
            try
            {
                var result = await _service.RefreshTokenAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost("revoke_token")]
        public async Task<IActionResult> RevokeToken([FromBody] RefreshTokenDTO model)
        {
            var revoked = await _service.RevokeTokenAsync(model.RefreshToken);
            if (!revoked)
                return BadRequest(new { message = "Token is invalid or already revoked" });

            return Ok(new { message = "Token revoked successfully" });
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserProfileDTO>> GetCurrentUser()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            var profile = await _service.GetUserProfileAsync(userId);
            return Ok(profile);
        }
    }
}
