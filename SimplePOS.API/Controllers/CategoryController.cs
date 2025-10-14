using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimplePOS.Business.DTOs;
using SimplePOS.Business.Interfaces;

namespace SimplePOS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        //GET: api/Category
        [HttpGet]
        [Authorize(Roles ="Admin,Empleado")]
        public async Task<ActionResult<List<CategoryReadDto>>> GetAllAsync()
        {
            var categories = await categoryService.GetAllAsync();
            return Ok(categories);
        }

        //GET: api/Category/5
        [HttpGet("{id}", Name = "GetCategoryById")]
        [Authorize(Roles = "Admin,Empleado")]
        public async Task<ActionResult<CategoryReadDto>> GetById(int id)
        {
            var category = await categoryService.GetByIdAsync(id);
            return Ok(category);
        }

        //POST: api/Category
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CategoryReadDto>> CreateAsync(CategoryCreateDto categoryCreateDto)
        {
            var category = await categoryService.CreateAsync(categoryCreateDto);
            return CreatedAtRoute("GetCategoryById", new { id = category.Id }, category);
        }

        //PUT: api/Category/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, CategoryUpdateDto categoryUpdateDto)
        {
            await categoryService.UpdateAsync(id, categoryUpdateDto);
            return NoContent();
        }

        //DELETE: api/Category/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await categoryService.DeleteAsync(id);
            return NoContent();
        }
    }
}
