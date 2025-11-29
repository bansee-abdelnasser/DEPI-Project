using Eventa.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Eventa.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizerController : ControllerBase
    {


        private readonly IOrganizerService _service;

        public OrganizerController(IOrganizerService service)
        {
            _service = service;
        }
        [Authorize(Roles = "organizers")]
        [HttpGet("my-events")]
        public async Task<IActionResult> GetMyEventsSummary()
        {
            var organizerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(organizerId))
                return Unauthorized();

            var summary = await _service.GetOrganizerEventsSummaryAsync(organizerId);
            return Ok(summary);
        }

        [Authorize]
        [HttpPost("rate-organizer/{organizerId}")]
        public async Task<IActionResult> RateOrganizer(string organizerId, [FromBody] int rating)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var success = await _service.RateOrganizerAsync(userId, organizerId, rating);
            if (!success)
                return BadRequest("Unable to rate organizer.");

            return Ok("Rating submitted successfully.");
        }

    }
}
