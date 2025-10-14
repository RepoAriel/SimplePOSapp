using System.ComponentModel.DataAnnotations;

namespace SimplePOS.Business.DTOs
{
    public class ProductUpdateDto
    {
        [Required(ErrorMessage = "El ID del producto es obligatorio.")]
        public int Id { get; set; }

        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string? Name { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres.")]
        public string? Description { get; set; }

        [Range(0.01, 100000, ErrorMessage = "El precio debe ser mayor a 0.")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
        public int Stock { get; set; }

        public bool IsActive { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría válida.")]
        public int CategoryId { get; set; }
    }
}