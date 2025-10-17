using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimplePOS.Business.DTOs;
using SimplePOS.Business.Interfaces;
using SimplePOS.Domain;

namespace SimplePOS.API.Controllers
{
    /// <summary>
    /// Controlador para gestionar los productos.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly ISaleService saleService;
        public SaleController(ISaleService saleService)
        {
            this.saleService = saleService;
        }

        /// <summary>
        /// Obtiene una lista paginada de ventas, con opción de buscar por nombre, correo o teléfono del cliente.
        /// </summary>
        /// <param name="paginationParams">Parámetros de paginación (página, tamaño).</param>
        /// <param name="searchTerm">Término de búsqueda para filtrar por cliente.</param>
        /// <returns>Lista paginada de ventas.</returns>
        /// <response code="200">Ventas obtenidas correctamente.</response>
        //GET: api/Sale/paged?page=1&pageSize=10
        [HttpGet("paged")]
        [Authorize(Roles = "Admin,Empleado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<SaleReadDto>>> GetPagedSales(
            [FromQuery] PaginationParams paginationParams,
            [FromQuery] string searchTerm ="")
        {
            var result = await saleService.GetPagedSalesAsync(paginationParams, searchTerm);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene todas las ventas registradas.
        /// </summary>
        /// <returns>Lista de ventas.</returns>
        /// <response code="200">Ventas obtenidas correctamente.</response>
        //GET: api/Sale
        [HttpGet]
        [Authorize(Roles = "Admin,Empleado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<SaleReadDto>>> GetAll()
        {
            var sales = await saleService.GetSalesAsync();
            return Ok(sales);
        }

        /// <summary>
        /// Obtiene los detalles de una venta por su ID.
        /// </summary>
        /// <param name="id">ID de la venta.</param>
        /// <returns>Venta correspondiente al ID.</returns>
        /// <response code="200">Venta encontrada.</response>
        /// <response code="404">Venta no encontrada.</response>
        //GET: api/Sale/5
        [HttpGet("{id}", Name = "GetSaleById")]
        [Authorize(Roles = "Admin,Empleado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SaleReadDto>> GetById(int id)
        {
            var sale = await saleService.GetSaleByIdAsync(id);
            return Ok(sale);
        }

        /// <summary>
        /// Genera y descarga un archivo PDF con la factura de una venta.
        /// </summary>
        /// <param name="id">ID de la venta.</param>
        /// <returns>Archivo PDF con la factura.</returns>
        /// <response code="200">Factura generada correctamente.</response>
        /// <response code="404">Venta no encontrada.</response>
        /// <response code="500">Error al generar la factura.</response>
        //GET: api/Sale/id/factura
        [HttpGet("{id}/factura")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Registra una nueva venta.
        /// </summary>
        /// <param name="saleCreateDto">Datos de la venta a registrar.</param>
        /// <returns>Venta registrada.</returns>
        /// <response code="201">Venta registrada correctamente.</response>
        //POST: api/Sale
        [HttpPost]
        [Authorize(Roles = "Admin,Empleado")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<SaleReadDto>> Create(SaleCreateDto saleCreateDto)
        {
            var sale = await saleService.RegisterSaleAsync(saleCreateDto);
            return CreatedAtRoute("GetSaleById", new { id = sale.Id }, sale);
        }
    }
}
