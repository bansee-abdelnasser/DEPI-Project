using Eventa.Application.DTOs.User;
using Eventa.Application.Interfaces;
using Eventa.DataAccess.Entities;
using Eventa.DataAccess.Repositories.Todo.DataAccess.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Eventa.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenManager _tokenManager;
        private readonly SignInManager<AppUser> _signInManager;

        public UserController(
            IUserService service,
            UserManager<AppUser> userManager,
            ITokenManager tokenManager,
            SignInManager<AppUser> signInManager)
        {
            _service = service;
            _userManager = userManager;
            _tokenManager = tokenManager;
            _signInManager = signInManager;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterDto user)
        {

            return Ok(await _service.Register(user));
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDto user)
        {
            var result = await _service.LoginAsync(user);
            if (result.Succeeded)
            {
                return Ok(new { result.TokenResult, result.UserName, Claims = result.Claims });
            }
            return Unauthorized(new { user.Username, Errors = result.Errors.ToArray() });
        }

        [HttpGet("external-login")]
        public IActionResult ExternalLogin(string provider, string returnUrl = "/")
        {
            // The redirectUrl is the callback endpoint that Google will call after login
            var redirectUrl = Url.Action("ExternalLoginCallback", "User", new { returnUrl }, Request.Scheme);
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }




        [HttpGet("external-login-callback")]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return BadRequest("External login failed");

            var signInResult = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider,
                info.ProviderKey,
                isPersistent: false);

            AppUser user;

            if (!signInResult.Succeeded)
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                user = new AppUser
                {
                    UserName = email,
                    Email = email,
                    FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName),
                    LastName = info.Principal.FindFirstValue(ClaimTypes.Surname) ?? "User",
                    IsExternalUser = true

                };

                await _userManager.CreateAsync(user);
                await _userManager.AddLoginAsync(user, info);
                await _userManager.AddToRoleAsync(user, "users");
            }
            else
            {
                user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            }

            var tokenResult = await _tokenManager.GetTokenAsync(user);

            // Return JSON instead of redirect
            return Ok(new
            {
                user.UserName,
                Token = tokenResult.Token,
                TokenExpiry = tokenResult.TokenExpiryTime
            });
        }



        [Authorize]
        [HttpPut("edit-profile")]
        public async Task<IActionResult> EditProfile([FromForm] UpdateProfileDto dto)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _service.UpdateProfileAsync(userId, dto);
            return Ok(result);
        }

        [Authorize]
        [HttpDelete("DeleteProfilePicture")]
        public async Task<IActionResult> DeleteProfilePicture()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _service.DeleteProfilePictureAsync(userId);

            if (!result)
                return BadRequest("No profile picture to delete.");

            return Ok("Profile picture deleted successfully.");
        }

        [Authorize]
        [HttpPost("toggle-role")]
        public async Task<IActionResult> ToggleRole()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            // Get the current user
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User not found.");

            // Check current roles
            var roles = await _userManager.GetRolesAsync(user);

            IdentityResult result;

            if (roles.Contains("organizers"))
            {
                // Switch to user
                result = await _service.SwitchToUserAsync(userId);
            }
            else
            {
                // Switch to organizer
                result = await _service.SwitchToOrganizerAsync(userId);
            }

            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => e.Description));

            // Generate new token with updated role
            var token = await _tokenManager.GetTokenAsync(user);

            var newRole = roles.Contains("organizers") ? "user" : "organizer";

            return Ok(new { message = $"Role switched to {newRole} successfully.", token });
        }


    }
}
