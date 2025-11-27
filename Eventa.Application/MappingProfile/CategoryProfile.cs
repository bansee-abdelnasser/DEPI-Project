using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Eventa.Application.DTOs.Category;
using Eventa.DataAccess.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Eventa.Application.MappingProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<Category, CreateCategoryDto>(); // optional
            CreateMap<CategoryDto, Category>();
            CreateMap<CreateCategoryDto, Category>(); // duplicate-safe
        }
    }
}
