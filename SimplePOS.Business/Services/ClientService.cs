using AutoMapper;
using SimplePOS.Business.DTOs;
using SimplePOS.Business.Interfaces;
using SimplePOS.Domain.Entities;
using SimplePOS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePOS.Business.Services
{
    internal class ClientService : IClientService
    {
        private readonly IGenericRepository<Client> clientRepo;
        private readonly IMapper mapper;

        public ClientService(IGenericRepository<Client> clientRepo, IMapper mapper)
        {
            this.clientRepo = clientRepo;
            this.mapper = mapper;
        }
        public async Task<ClientReadDto> CreateAsync(ClientCreateDto clientCreateDto)
        {
            var client = mapper.Map<Client>(clientCreateDto);
            await clientRepo.AddAsync(client);
            await clientRepo.SaveChangesAsync();
            return mapper.Map<ClientReadDto>(client);
        }

        public async Task DeleteAsync(int id)
        {
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
                throw new Exception("Cliente no encontrado");
            return mapper.Map<ClientReadDto>(client);
        }

        public async Task UpdateAsync(int id, ClientUpdateDto clientUpdateDto)
        {
            var client = await clientRepo.GetByIdAsync(id);
            if(client == null)
                throw new Exception("Cliente no encontrado");   

            mapper.Map(clientUpdateDto, client);
            clientRepo.Update(client);
            await clientRepo.SaveChangesAsync();
        }
    }
}
