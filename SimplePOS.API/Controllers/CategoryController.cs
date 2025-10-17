using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimplePOS.Business.DTOs;
using SimplePOS.Business.Interfaces;

namespace SimplePOS.API.Controllers
{
    /// <summary>
    /// Controlador para gestionar las categorías de productos.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        /// <summary>
        /// Obtiene todas las categorías de productos.
        /// </summary>
        /// <returns>Lista de categorías</returns>
        /// <response code="200">Devuelve la lista de categorías</response>
        //GET: api/Category
        [HttpGet]
        [Authorize(Roles ="Admin,Empleado")]
        [ProducesResponseType(typeof(List<CategoryReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<CategoryReadDto>>> GetAllAsync()
        {
            var categories = await categoryService.GetAllAsync();
            return Ok(categories);
        }

        /// <summary>
        /// Obtiene una categoría por su ID
        /// </summary>
        /// <param name="id">ID de la categoría</param>
        /// <returns>La categoría solicitada</returns>
        /// <response code="200">Devuelve la categoría solicitada</response>
        /// <response code="404">Categoría no encontrada</response>
        //GET: api/Category/5
        [HttpGet("{id}", Name = "GetCategoryById")]
        [Authorize(Roles = "Admin,Empleado")]
        [ProducesResponseType(typeof(CategoryReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryReadDto>> GetById(int id)
        {
            var category = await categoryService.GetByIdAsync(id);
            return Ok(category);
        }


        /// <summary>
        /// Crea una nueva categoría.
        /// </summary>
        /// <param name="categoryCreateDto">Datos de la nueva categoría.</param>
        /// <returns>La categoría creada.</returns>
        /// <response code="201">Categoría creada correctamente.</response>
        /// <response code="400">Datos inválidos.</response>
        //POST: api/Category
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(CategoryReadDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CategoryReadDto>> CreateAsync(CategoryCreateDto categoryCreateDto)
        {
            var category = await categoryService.CreateAsync(categoryCreateDto);
            return CreatedAtRoute("GetCategoryById", new { id = category.Id }, category);
        }

        /// <summary>
        /// Actualiza una categoría existente.
        /// </summary>
        /// <param name="id">ID de la categoría a actualizar.</param>
        /// <param name="categoryUpdateDto">Datos actualizados de la categoría.</param>
        /// <returns>Sin contenido.</returns>
        /// <response code="204">Actualización exitosa.</response>
        /// <response code="400">Datos inválidos.</response>
        /// <response code="404">Categoría no encontrada.</response>
        //PUT: api/Category/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAsync(int id, CategoryUpdateDto categoryUpdateDto)
        {
            await categoryService.UpdateAsync(id, categoryUpdateDto);
            return NoContent();
        }

        /// <summary>
        /// Elimina una categoría por su ID.
        /// </summary>
        /// <param name="id">ID de la categoría a eliminar.</param>
        /// <returns>Sin contenido.</returns>
        /// <response code="204">Eliminación exitosa.</response>
        /// <response code="404">Categoría no encontrada.</response>
        //DELETE: api/Category/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await categoryService.DeleteAsync(id);
            return NoContent();
        }
    }
}
