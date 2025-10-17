using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using SimplePOS.Business.DTOs;
using SimplePOS.Business.Interfaces;
using SimplePOS.Domain;

namespace SimplePOS.API.Controllers
{
    /// <summary>
    /// Controlador para gestionar los clientes.
    /// </summary>
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

        /// <summary>
        /// Obtiene una lista paginada de clientes, con filtro opcional por término de búsqueda.
        /// </summary>
        /// <param name="paginationParams">Parámetros de paginación (página, tamaño).</param>
        /// <param name="searchTerm">Término de búsqueda opcional (nombre, email o teléfono).</param>
        /// <returns>Lista paginada de clientes.</returns>
        /// <response code="200">Lista de clientes paginada obtenida correctamente.</response>
        //GET: api/Client/paged?page=1&pageSize=10
        [HttpGet("paged")]
        [Authorize(Roles = "Admin,Empleado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<ClientReadDto>>> GetPagedClients(
            [FromQuery] PaginationParams paginationParams,
            [FromQuery] string searchTerm = "")
        {
            var result = await clientService.GetPagedClientsAsync(paginationParams, searchTerm);
            return Ok(result);
        }


        /// <summary>
        /// Obtiene todos los clientes sin paginar.
        /// </summary>
        /// <returns>Lista completa de clientes.</returns>
        /// <response code="200">Clientes obtenidos correctamente.</response>
        //GET: api/Client
        [HttpGet]
        [Authorize(Roles = "Admin,Empleado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ClientReadDto>>> GetAll()
        {
            var clients = await clientService.GetAllAsync();
            return Ok(clients);
        }

        /// <summary>
        /// Obtiene un cliente por su ID.
        /// </summary>
        /// <param name="id">ID del cliente.</param>
        /// <returns>Cliente con el ID especificado.</returns>
        /// <response code="200">Cliente encontrado.</response>
        /// <response code="404">Cliente no encontrado.</response>
        //GET: api/Client/5
        [HttpGet("{id}", Name = "GetClientById")]
        [Authorize(Roles = "Admin,Empleado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClientReadDto>> GetById(int id)
        {
            var client = await clientService.GetByIdAsync(id);
            if (client == null)
                return NotFound(new { message = "Cliente no encontrado" });
            return Ok(client);
        }

        /// <summary>
        /// Crea un nuevo cliente (con carga opcional de imagen).
        /// </summary>
        /// <param name="clientCreateDto">Datos del cliente a crear.</param>
        /// <returns>Cliente creado.</returns>
        /// <response code="201">Cliente creado correctamente.</response>
        //POST: api/Client
        [HttpPost]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "Admin, Empleado")]
        [ProducesResponseType(StatusCodes.Status201Created)]
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

        /// <summary>
        /// Actualiza los datos de un cliente.
        /// </summary>
        /// <param name="id">ID del cliente a actualizar.</param>
        /// <param name="clientUpdateDto">Datos nuevos del cliente.</param>
        /// <returns>NoContent.</returns>
        /// <response code="204">Cliente actualizado correctamente.</response>
        //PUT: api/Client/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Empleado")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(int id, ClientUpdateDto clientUpdateDto)
        {
            await clientService.UpdateAsync(id, clientUpdateDto);
            return NoContent();
        }

        /// <summary>
        /// Elimina un cliente por su ID.
        /// </summary>
        /// <param name="id">ID del cliente.</param>
        /// <returns>NoContent.</returns>
        /// <response code="204">Cliente eliminado correctamente.</response>
        //DELETE: api/Client/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int id)
        {
            await clientService.DeleteAsync(id);
            return NoContent();
        }
    }
}
