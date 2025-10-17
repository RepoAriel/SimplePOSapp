using System.ComponentModel.DataAnnotations;

namespace SimplePOS.Business.DTOs
{
    /// <summary>
    /// Representa la solicitud para crear una nueva categoría.
    /// </summary>
    public class CategoryCreateDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}