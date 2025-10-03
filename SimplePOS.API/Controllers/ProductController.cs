using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimplePOS.Business.DTOs;
using SimplePOS.Business.Interfaces;
using SimplePOS.Business.Services;
using SimplePOS.Domain;

namespace SimplePOS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }
        //GET: api/Product/paged?page=1&pageSize=10
        [HttpGet("paged")]
        public async Task<ActionResult<PagedResult<ProductReadDto>>> GetProducts(
            [FromQuery] PaginationParams paginationParams,
            [FromQuery] string searchTerm)
        {
            var result = await productService.GetPagedProductsAsync(paginationParams, searchTerm);
            return Ok(result);
        }

        //GET: api/Product
        [HttpGet]
        public async Task<ActionResult<List<ProductReadDto>>> GetAllAsync()
        {
            var products = await productService.GetAllAsync();
            return Ok(products);
        }

        //GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductReadDto>> GetByIdAsync(int id)
        {
            var product = await productService.GetByIdAsync(id);
            return Ok(product);
        }

        //POST: api/Product
        [HttpPost]
        public async Task<ActionResult<ProductReadDto>> CreateAsync(ProductCreateDto productCreateDto)
        {
            var product = await productService.CreateAsync(productCreateDto);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = product.Id }, product);
        }

        //PUT: api/Product/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, ProductUpdateDto productUpdateDto)
        {
            await productService.UpdateAsync(id, productUpdateDto);
            return NoContent();
        }

        //DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await productService.DeleteAsync(id);
            return NoContent();
        }   
    }
}
