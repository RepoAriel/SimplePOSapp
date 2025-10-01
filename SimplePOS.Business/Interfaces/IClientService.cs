using SimplePOS.Business.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePOS.Business.Interfaces
{
    public interface IClientService
    {
        Task<List<ClientReadDto>> GetAllAsync();
        Task<ClientReadDto> GetByIdAsync(int id);
        Task<ClientReadDto> CreateAsync(ClientCreateDto clientCreateDto);
        Task UpdateAsync(int id, ClientUpdateDto clientUpdateDto);
        Task DeleteAsync(int id);
    }
}
