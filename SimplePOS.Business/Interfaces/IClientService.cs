using SimplePOS.Business.DTOs;
using SimplePOS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePOS.Business.Interfaces
{
    public interface IClientService
    {
        Task<PagedResult<ClientReadDto>> GetPagedClientsAsync(
            PaginationParams paginationParams, 
            string searchTerm);
        Task<List<ClientReadDto>> GetAllAsync();
        Task<ClientReadDto> GetByIdAsync(int id);
        Task<ClientReadDto> CreateAsync(ClientCreateDto clientCreateDto);
        Task UpdateAsync(int id, ClientUpdateDto clientUpdateDto);
        Task DeleteAsync(int id);
    }
}
