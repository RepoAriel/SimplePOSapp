using System.ComponentModel.DataAnnotations;

namespace SimplePOS.Business.DTOs
{
    public class CategoryCreateDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}