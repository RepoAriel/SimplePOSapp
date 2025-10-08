using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using SimplePOS.Business.DTOs;
using SimplePOS.Business.Interfaces;
using SimplePOS.Domain;

namespace SimplePOS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService clientService;
        private readonly IWebHostEnvironment env;

        public ClientController(IClientService clientService, IWebHostEnvironment env)
        {
            this.clientService = clientService;
            this.env = env;
        }

        //GET: api/Client/paged?page=1&pageSize=10
        [HttpGet("paged")]
        public async Task<ActionResult<PagedResult<ClientReadDto>>> GetPagedClients(
            [FromQuery] PaginationParams paginationParams,
            [FromQuery] string searchTerm = "")
        {
            var result = await clientService.GetPagedClientsAsync(paginationParams, searchTerm);
            return Ok(result);
        }

        //GET: api/Client
        [HttpGet]
        public async Task<ActionResult<List<ClientReadDto>>> GetAll()
        {
            var clients = await clientService.GetAllAsync();
            return Ok(clients);
        }

        //GET: api/Client/5
        [HttpGet("{id}", Name = "GetClientById")]
        public async Task<ActionResult<ClientReadDto>> GetById(int id)
        {
            var client = await clientService.GetByIdAsync(id);
            if (client == null)
                return NotFound(new { message = "Cliente no encontrado" });
            return Ok(client);
        }

        //POST: api/Client
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ClientReadDto>> Create([FromForm]ClientCreateDto clientCreateDto)
        {
            string photoUrl = null;

            //Guardar foto si existe
            if(clientCreateDto.PhotoFile != null)
            {
                //Generando nombre de archivo unico
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(clientCreateDto.PhotoFile.FileName);
                
                //Definir ruta de guardado (wwwroot/images/clients)
                var uploadsFolder = Path.Combine(env.WebRootPath, "images", "clients");
                Directory.CreateDirectory(uploadsFolder);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                //Guardar archivo
                using(var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await clientCreateDto.PhotoFile.CopyToAsync(fileStream);
                }

                //Crear URL accesible publicamente
                photoUrl = $"/images/clients/{uniqueFileName}";
            }

            //Pasar DTO y URL Generada al servicio ClientService
            var client = await clientService.CreateAsync(clientCreateDto, photoUrl);
            return CreatedAtRoute("GetClientById", new { id = client.Id }, client);
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
