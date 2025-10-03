using SimplePOS.Business.DTOs;
using SimplePOS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePOS.Business.Interfaces
{
    public interface IProductService
    {
        Task<PagedResult<ProductReadDto>> GetPagedProductsAsync(PaginationParams paginationParams, string searchTerm);
        Task<List<ProductReadDto>> GetAllAsync();
        Task<ProductReadDto> GetByIdAsync(int id);
        Task<ProductReadDto> CreateAsync(ProductCreateDto productCreateDto);
        Task UpdateAsync(int id, ProductUpdateDto productUpdateDto);
        Task DeleteAsync(int id);
    }
}
