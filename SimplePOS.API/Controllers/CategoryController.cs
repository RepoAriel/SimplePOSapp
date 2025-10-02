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
        public async Task<ActionResult<List<CategoryReadDto>>> GetAllAsync()
        {
            var categories = await categoryService.GetAllAsync();
            return Ok(categories);
        }

        //GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryReadDto>> GetByIdAsync(int id)
        {
            var category = await categoryService.GetByIdAsync(id);
            return Ok(category);
        }

        //POST: api/Category
        [HttpPost]
        public async Task<ActionResult<CategoryReadDto>> CreateAsync(CategoryCreateDto categoryCreateDto)
        {
            var category = await categoryService.CreateAsync(categoryCreateDto);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = category.Id }, category);
        }

        //PUT: api/Category/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, CategoryUpdateDto categoryUpdateDto)
        {
            await categoryService.UpdateAsync(id, categoryUpdateDto);
            return NoContent();
        }

        //DELETE: api/Category/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await categoryService.DeleteAsync(id);
            return NoContent();
        }
    }
}
