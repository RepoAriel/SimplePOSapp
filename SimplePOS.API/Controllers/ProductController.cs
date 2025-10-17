using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimplePOS.Business.DTOs;
using SimplePOS.Business.Interfaces;
using SimplePOS.Business.Services;
using SimplePOS.Domain;

namespace SimplePOS.API.Controllers
{
    /// <summary>
    /// Controlador para gestionar los productos.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly IWebHostEnvironment env;

        public ProductController(IProductService productService, IWebHostEnvironment env)
        {
            this.productService = productService;
            this.env = env;
        }

        /// <summary>
        /// Obtiene una lista paginada de productos con un filtro opcional por nombre.
        /// </summary>
        /// <param name="paginationParams">Parámetros de paginación (página, tamaño).</param>
        /// <param name="searchTerm">Término de búsqueda (nombre del producto).</param>
        /// <returns>Lista paginada de productos.</returns>
        /// <response code="200">Productos obtenidos correctamente.</response>
        //GET: api/Product/paged?page=1&pageSize=10
        [HttpGet("paged")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<ProductReadDto>>> GetProducts(
            [FromQuery] PaginationParams paginationParams,
            [FromQuery] string searchTerm = "")
        {
            var result = await productService.GetPagedProductsAsync(paginationParams, searchTerm);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene todos los productos sin paginación.
        /// </summary>
        /// <returns>Lista de todos los productos.</returns>
        /// <response code="200">Productos obtenidos correctamente.</response>
        //GET: api/Product
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ProductReadDto>>> GetAllAsync()
        {
            var products = await productService.GetAllAsync();
            return Ok(products);
        }

        /// <summary>
        /// Obtiene un producto por su ID.
        /// </summary>
        /// <param name="id">ID del producto.</param>
        /// <returns>Producto correspondiente al ID.</returns>
        /// <response code="200">Producto encontrado.</response>
        /// <response code="404">Producto no encontrado.</response>
        //GET: api/Product/5
        [HttpGet("{id}", Name = "GetProductById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductReadDto>> GetById(int id)
        {
            var product = await productService.GetByIdAsync(id);
            return Ok(product);
        }

        /// <summary>
        /// Crea un nuevo producto (con carga opcional de imagen).
        /// </summary>
        /// <param name="productCreateDto">Datos del producto a crear.</param>
        /// <returns>Producto creado.</returns>
        /// <response code="201">Producto creado exitosamente.</response>
        //POST: api/Product
        [HttpPost]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<ProductReadDto>> CreateAsync([FromForm]ProductCreateDto productCreateDto)
        {
            string photoUrl = null;
            //Guardar foto si existe
            if (productCreateDto.PhotoFile != null)
            {
                //Generar nombre de archivo unico
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(productCreateDto.PhotoFile.FileName);
                //Definir ruta de guardado (wwwroot/images/products)
                var uploadsFolder = Path.Combine(env.WebRootPath, "images", "products");
                Directory.CreateDirectory(uploadsFolder);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                //Guardar archivo
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await productCreateDto.PhotoFile.CopyToAsync(fileStream);
                }
                //Crear URL accesible publicamente
                photoUrl = $"/images/products/{uniqueFileName}";
            }
            var product = await productService.CreateAsync(productCreateDto, photoUrl);
            return CreatedAtRoute("GetProductById", new { id = product.Id }, product);
        }

        /// <summary>
        /// Actualiza los datos de un producto existente.
        /// </summary>
        /// <param name="id">ID del producto a actualizar.</param>
        /// <param name="productUpdateDto">Datos actualizados del producto.</param>
        /// <returns>NoContent.</returns>
        /// <response code="204">Producto actualizado correctamente.</response>
        //PUT: api/Product/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateAsync(int id, ProductUpdateDto productUpdateDto)
        {
            await productService.UpdateAsync(id, productUpdateDto);
            return NoContent();
        }

        /// <summary>
        /// Elimina un producto por su ID.
        /// </summary>
        /// <param name="id">ID del producto a eliminar.</param>
        /// <returns>NoContent.</returns>
        /// <response code="204">Producto eliminado correctamente.</response>
        //DELETE: api/Product/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await productService.DeleteAsync(id);
            return NoContent();
        }   
    }
}
