using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Eventa.Application.Interfaces;
using Eventa.DataAccess.Entities;
using Eventa.Application.DTOs.Event;


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

        [HttpPost]
        public IActionResult Create(CreateEventDto dto)
        {
            var created = _service.CreateEvent(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateEventDto dto)
        {
            var ok = _service.UpdateEvent(id, dto);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var ok = _service.DeleteEvent(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }

}

