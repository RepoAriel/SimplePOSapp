using System.ComponentModel.DataAnnotations;

namespace SimplePOS.Business.DTOs
{
    public class SaleCreateDto
    {
        public int ClientId { get; set; }
        [Required(ErrorMessage = "La venta debe contener al menos un item")]
        [MinLength(1, ErrorMessage = "La venta debe contener al menos un item")]
        public List<SaleItemCreateDto> Items { get; set; } = new();
    }
}