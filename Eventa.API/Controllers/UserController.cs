using Eventa.Application.DTOs.User;
using Eventa.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eventa.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
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


    }
}
