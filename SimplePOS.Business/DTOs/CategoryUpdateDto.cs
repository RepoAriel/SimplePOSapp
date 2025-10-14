using System.ComponentModel.DataAnnotations;

namespace SimplePOS.Business.DTOs
{
    public class CategoryUpdateDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}