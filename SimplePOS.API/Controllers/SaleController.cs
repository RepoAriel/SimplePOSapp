using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimplePOS.Business.DTOs;
using SimplePOS.Business.Interfaces;
using SimplePOS.Domain;

namespace SimplePOS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly ISaleService saleService;
        public SaleController(ISaleService saleService)
        {
            this.saleService = saleService;
        }

        //GET: api/Sale/paged?page=1&pageSize=10
        [HttpGet("paged")]
        [Authorize(Roles = "Admin,Empleado")]
        public async Task<ActionResult<PagedResult<SaleReadDto>>> GetPagedSales(
            [FromQuery] PaginationParams paginationParams,
            [FromQuery] string searchTerm)
        {
            var result = await saleService.GetPagedSalesAsync(paginationParams, searchTerm);
            return Ok(result);
        }

        //GET: api/Sale
        [HttpGet]
        [Authorize(Roles = "Admin,Empleado")]
        public async Task<ActionResult<List<SaleReadDto>>> GetAll()
        {
            var sales = await saleService.GetSalesAsync();
            return Ok(sales);
        }

        //GET: api/Sale/5
        [HttpGet("{id}", Name = "GetSaleById")]
        [Authorize(Roles = "Admin,Empleado")]
        public async Task<ActionResult<SaleReadDto>> GetById(int id)
        {
            var sale = await saleService.GetSaleByIdAsync(id);
            return Ok(sale);
        }

        //GET: api/Sale/id/factura
        [HttpGet("{id}/factura")]
        public async Task<IActionResult> DescargarFactura(int id)
        {
            try
            {
                var sale = await saleService.GetSaleByIdAsync(id);

                if (sale == null)
                    return NotFound();

                var pdfBytes = saleService.GenerarFactura(sale);

                return File(pdfBytes, "application/pdf", $"factura_{id}.pdf");
            }
            catch (Exception ex)
            {
                // Puedes usar logger aquí para registrar el error real
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }


        //POST: api/Sale
        [HttpPost]
        [Authorize(Roles = "Admin,Empleado")]
        public async Task<ActionResult<SaleReadDto>> Create(SaleCreateDto saleCreateDto)
        {
            var sale = await saleService.RegisterSaleAsync(saleCreateDto);
            return CreatedAtRoute("GetSaleById", new { id = sale.Id }, sale);
        }
    }
}
