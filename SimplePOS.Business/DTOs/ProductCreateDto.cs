using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SimplePOS.Business.DTOs
{
    public class ProductCreateDto
    {
        [Required]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? PhotoFile { get; set; }   
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
    }
}