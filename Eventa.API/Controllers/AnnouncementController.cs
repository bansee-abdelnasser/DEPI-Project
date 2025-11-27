using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Eventa.Application.Interfaces;
using Eventa.DataAccess.Entities;
using Eventa.Application.DTOs.Announcement;

namespace Eventa.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementController : ControllerBase
    {
        private readonly IAnnouncementService _service;

        public AnnouncementController(IAnnouncementService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var a = _service.GetById(id);
            if (a == null) return NotFound();
            return Ok(a);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateAnnouncementDto dto)
        {
            var newA = _service.Create(dto);
            return Ok(newA);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var res = _service.Delete(id);
            if (!res) return NotFound();
            return Ok("Deleted");
        }
    }
}
