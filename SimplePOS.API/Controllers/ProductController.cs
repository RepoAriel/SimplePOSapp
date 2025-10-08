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
        private readonly IWebHostEnvironment env;

        public ProductController(IProductService productService, IWebHostEnvironment env)
        {
            this.productService = productService;
            this.env = env;
        }
        //GET: api/Product/paged?page=1&pageSize=10
        [HttpGet("paged")]
        public async Task<ActionResult<PagedResult<ProductReadDto>>> GetProducts(
            [FromQuery] PaginationParams paginationParams,
            [FromQuery] string searchTerm = "")
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
        [HttpGet("{id}", Name = "GetProductById")]
        public async Task<ActionResult<ProductReadDto>> GetById(int id)
        {
            var product = await productService.GetByIdAsync(id);
            return Ok(product);
        }

        //POST: api/Product
        [HttpPost]
        [Consumes("multipart/form-data")]
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
