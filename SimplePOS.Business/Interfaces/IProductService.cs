using SimplePOS.Business.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePOS.Business.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductReadDto>> GetAllAsync();
        Task<ProductReadDto> GetByIdAsync(int id);
        Task<ProductReadDto> CreateAsync(ProductCreateDto productCreateDto);
        Task UpdateAsync(int id, ProductUpdateDto productUpdateDto);
        Task DeleteAsync(int id);
    }
}
