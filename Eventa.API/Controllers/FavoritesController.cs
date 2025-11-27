using Eventa.Application.DTOs.User;
using Eventa.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eventa.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FavoritesController : ControllerBase
    {
        private readonly IFavoriteService _service;

        public FavoritesController(IFavoriteService service)
        {
            _service = service;
        }


        [HttpGet("favorites")]
        public async Task<IActionResult> GetUserFavoritesAsync()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var favorites = await _service.GetUserFavoritesAsync(userId);
            return Ok(favorites);
        }

        [HttpPost("toggle")]
        public async Task<IActionResult> ToggleFavoriteAsync(int eventId)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _service.ToggleFavoriteAsync(eventId,userId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
