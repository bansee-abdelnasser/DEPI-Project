using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eventa.Application.Interfaces;
using Eventa.DataAccess.Entities;
using Eventa.DataAccess.Interfaces;
using AutoMapper;
using Eventa.Application.DTOs.Category;
using System.Collections.Generic;
using System.Linq;

namespace Eventa.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        public IEnumerable<CategoryDto> GetAll()
        {
            return _unit.Categories.GetAll().Select(c => _mapper.Map<CategoryDto>(c));
        }

        public CategoryDto? GetById(int id)
        {
            var c = _unit.Categories.FindById(id);
            if (c == null) return null;
            return _mapper.Map<CategoryDto>(c);
        }

        public CategoryDto Create(CreateCategoryDto dto)
        {
            var cat = _mapper.Map<Category>(dto);
            _unit.Categories.Create(cat);
            _unit.Save();
            return _mapper.Map<CategoryDto>(cat);
        }

        public bool Delete(int id)
        {
            var deleted = _unit.Categories.Delete(id);
            if (deleted == null) return false;
            _unit.Save();
            return true;
        }
    }
}
