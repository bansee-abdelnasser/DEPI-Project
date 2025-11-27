using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eventa.DataAccess.Entities;
using Eventa.Application.DTOs.Category;
using System.Collections.Generic;
namespace Eventa.Application.Interfaces
{
    public interface ICategoryService
    {
        IEnumerable<CategoryDto> GetAll();
        CategoryDto GetById(int id);
        CategoryDto Create(CreateCategoryDto categoryDto);
        bool Delete(int id);
    }
}
