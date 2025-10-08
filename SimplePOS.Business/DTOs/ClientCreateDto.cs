using Microsoft.AspNetCore.Http;

namespace SimplePOS.Business.DTOs
{
    public class ClientCreateDto
    {
        public string? Name { get; set; }
        public string? Document { get; set; }
        public IFormFile? PhotoFile { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}