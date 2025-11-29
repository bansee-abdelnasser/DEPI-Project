using Eventa.Application.DTOs.Event;
using Eventa.Application.Interfaces;
using Eventa.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Eventa.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IEventService _service;

        public EventController(IEventService service) => _service = service;

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAllEvents());

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var e = _service.GetEventById(id);
            if (e == null) return NotFound();
            return Ok(e);
        }
        [Authorize(Roles = "organizers")]
        [HttpPost]
        public IActionResult Create(CreateEventDto dto)
        {
            var organizerId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(organizerId))
                return Unauthorized();

            var created = _service.CreateEvent(dto, organizerId);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }
        [Authorize(Roles = "organizers")]
        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateEventDto dto)
        {
            var ok = _service.UpdateEvent(id, dto);
            if (!ok) return NotFound();
            return NoContent();
        }
        [Authorize(Roles = "organizers")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var ok = _service.DeleteEvent(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }

}

