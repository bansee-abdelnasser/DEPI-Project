using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Eventa.Application.Interfaces;
using Eventa.DataAccess.Entities;
using Eventa.Application.DTOs.Category;

namespace Eventa.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_categoryService.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var cat = _categoryService.GetById(id);
            if (cat == null) return NotFound();
            return Ok(cat);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateCategoryDto dto)
        {
            var created = _categoryService.Create(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var ok = _categoryService.Delete(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
