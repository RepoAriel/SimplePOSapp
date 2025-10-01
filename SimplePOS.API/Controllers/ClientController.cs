using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using SimplePOS.Business.DTOs;
using SimplePOS.Business.Interfaces;

namespace SimplePOS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService clientService;

        public ClientController(IClientService clientService)
        {
            this.clientService = clientService;
        }

        //GET: api/Client
        [HttpGet]
        public async Task<ActionResult<List<ClientReadDto>>> GetAll()
        {
            var clients = await clientService.GetAllAsync();
            return Ok(clients);
        }

        //GET: api/Client/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClientReadDto>> GetById(int id)
        {
            var client = await clientService.GetByIdAsync(id);
            if (client == null)
                return NotFound(new { message = "Cliente no encontrado" });
            return Ok(client);
        }

        //POST: api/Client
        [HttpPost]
        public async Task<ActionResult<ClientReadDto>> Create(ClientCreateDto clientCreateDto)
        {
            var client = await clientService.CreateAsync(clientCreateDto);
            return CreatedAtAction(nameof(GetById), new { id = client.Id }, client);
        }

        //PUT: api/Client/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ClientUpdateDto clientUpdateDto)
        {
            await clientService.UpdateAsync(id, clientUpdateDto);
            return NoContent();
        }

        //DELETE: api/Client/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await clientService.DeleteAsync(id);
            return NoContent();
        }
    }
}
