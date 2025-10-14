using System.ComponentModel.DataAnnotations;

namespace SimplePOS.Business.DTOs
{
    public class SaleItemUpdateDto
    {
        [Required(ErrorMessage = "El ID del ítem es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID debe ser mayor que cero.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El ID del producto es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del producto debe ser mayor que cero.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "El precio unitario es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor que 0.")]
        public decimal UnitPrice { get; set; }
    }
}