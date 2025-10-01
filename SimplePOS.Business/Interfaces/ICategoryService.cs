using SimplePOS.Business.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePOS.Business.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryReadDto>> GetAllAsync();
        Task<CategoryReadDto> GetByIdAsync(int id);
        Task<CategoryReadDto> CreateAsync(CategoryCreateDto categoryCreateDto);
        Task UpdateAsync(int id, CategoryUpdateDto categoryUpdateDto);
        Task DeleteAsync(int id);
    }
}
