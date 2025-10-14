using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SimplePOS.Business.DTOs
{
    public class ProductCreateDto
    {
        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string? Name { get; set; }

        [StringLength(500, ErrorMessage = "La descripci�n no puede superar los 500 caracteres.")]
        public string? Description { get; set; }

        public IFormFile? PhotoFile { get; set; }

        [Range(0.01, 100000, ErrorMessage = "El precio debe ser mayor a 0.")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
        public int Stock { get; set; }

        public bool IsActive { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categor�a v�lida.")]
        public int CategoryId { get; set; }
    }
}