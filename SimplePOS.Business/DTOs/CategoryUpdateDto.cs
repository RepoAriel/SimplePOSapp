using System.ComponentModel.DataAnnotations;

namespace SimplePOS.Business.DTOs
{
    /// <summary>
    /// Representa los datos necesarios para actualizar una categoría.
    /// </summary>
    public class CategoryUpdateDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}