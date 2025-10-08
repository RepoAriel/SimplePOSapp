using AutoMapper;
using SimplePOS.Business.DTOs;
using SimplePOS.Business.Exceptions;
using SimplePOS.Business.Interfaces;
using SimplePOS.Domain;
using SimplePOS.Domain.Entities;
using SimplePOS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SimplePOS.Business.Services
{
    internal class ClientService : IClientService
    {
        private readonly IGenericRepository<Client> clientRepo;
        private readonly IMapper mapper;
        private readonly IPaginationService paginationService;

        public ClientService(
            IGenericRepository<Client> clientRepo, 
            IMapper mapper,
            IPaginationService paginationService)
        {
            this.clientRepo = clientRepo;
            this.mapper = mapper;
            this.paginationService = paginationService;
        }
        public async Task<PagedResult<ClientReadDto>> GetPagedClientsAsync(
            PaginationParams paginationParams,
            string searchTerm)
        {
            Expression<Func<Client, bool>>? filter = null;
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var lowerTerm = searchTerm.ToLower();
                filter = c => 
                    (c.Name != null && c.Name.ToLower().Contains(lowerTerm)) ||
                    (c.Email != null && c.Email.ToLower().Contains(lowerTerm)) ||
                    (c.PhoneNumber != null && c.PhoneNumber.ToLower().Contains(lowerTerm)); 
            }
            string includes = "";
            return await paginationService.GetPagedAsync<Client, ClientReadDto>(
                paginationParams,
                clientRepo,
                filter,
                includeProperties: includes);
        }
        public async Task<ClientReadDto> CreateAsync(ClientCreateDto clientCreateDto, string? photoUrl)
        {
            var client = mapper.Map<Client>(clientCreateDto);
            client.PhotoURL = photoUrl;
            await clientRepo.AddAsync(client);
            await clientRepo.SaveChangesAsync();
            return mapper.Map<ClientReadDto>(client);
        }

        public async Task DeleteAsync(int id)
        {
            var client = await clientRepo.GetByIdAsync(id);
            if (client == null)
                throw new NotFoundException("Cliente no encontrado");
            await clientRepo.DeleteAsync(id);
            await clientRepo.SaveChangesAsync();
        }

        public async Task<List<ClientReadDto>> GetAllAsync()
        {
            var clients = await clientRepo.GetAllAsync();
            return mapper.Map<List<ClientReadDto>>(clients);
        }

        public async Task<ClientReadDto> GetByIdAsync(int id)
        {
            var client = await clientRepo.GetByIdAsync(id);
            if (client == null)
                throw new NotFoundException("Cliente no encontrado");
            return mapper.Map<ClientReadDto>(client);
        }

        public async Task UpdateAsync(int id, ClientUpdateDto clientUpdateDto)
        {
            var client = await clientRepo.GetByIdAsync(id);
            if(client == null)
                throw new NotFoundException("Cliente no encontrado");   

            mapper.Map(clientUpdateDto, client);
            clientRepo.Update(client);
            await clientRepo.SaveChangesAsync();
        }
    }
}
