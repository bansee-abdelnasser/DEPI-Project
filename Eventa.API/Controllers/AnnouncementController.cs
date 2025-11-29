using Eventa.Application.DTOs.Announcement;
using Eventa.Application.Interfaces;
using Eventa.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        [Authorize(Roles = "organizers")]
        [HttpPost]
        public IActionResult Create([FromBody] CreateAnnouncementDto dto)
        {
            var newA = _service.Create(dto);
            return Ok(newA);
        }
        [Authorize(Roles = "organizers")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var res = _service.Delete(id);
            if (!res) return NotFound();
            return Ok("Deleted");
        }
    }
}
