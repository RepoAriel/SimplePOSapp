using SimplePOS.Business.DTOs;
using SimplePOS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePOS.Business.Interfaces
{
    public interface ISaleService
    {
        Task<PagedResult<SaleReadDto>> GetPagedSalesAsync(
            PaginationParams paginationParams,
            string searchTerm);
        Task<SaleReadDto> RegisterSaleAsync(SaleCreateDto saleCreateDto);
        Task<List<SaleReadDto>> GetSalesAsync();
        Task<SaleReadDto?> GetSaleByIdAsync(int id);
        byte[] GenerarFactura(SaleReadDto sale);
    }
}
