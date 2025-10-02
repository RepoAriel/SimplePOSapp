using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimplePOS.Business.DTOs;
using SimplePOS.Business.Interfaces;

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

        //GET: api/Sale
        [HttpGet]
        public async Task<ActionResult<List<SaleReadDto>>> GetAll()
        {
            var sales = await saleService.GetSalesAsync();
            return Ok(sales);
        }

        //GET: api/Sale/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SaleReadDto>> GetById(int id)
        {
            var sale = await saleService.GetSaleByIdAsync(id);
            return Ok(sale);
        }

        //POST: api/Sale
        [HttpPost]
        public async Task<ActionResult<SaleReadDto>> Create(SaleCreateDto saleCreateDto)
        {
            var sale = await saleService.RegisterSaleAsync(saleCreateDto);
            return CreatedAtAction(nameof(GetById), new { id = sale.Id }, sale);
        }
    }
}
