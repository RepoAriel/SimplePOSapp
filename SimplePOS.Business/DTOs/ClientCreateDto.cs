using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SimplePOS.Business.DTOs
{
    /// <summary>
    /// Representa los datos necesarios para crear un nuevo cliente.
    /// </summary>
    public class ClientCreateDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string? Name { get; set; }

        [StringLength(20, ErrorMessage = "El documento no puede superar los 20 caracteres.")]
        public string? Document { get; set; }

        public IFormFile? PhotoFile { get; set; }

        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "El número de teléfono no es válido.")]
        [StringLength(20, ErrorMessage = "El número de teléfono no puede superar los 20 caracteres.")]
        public string? PhoneNumber { get; set; }
    }
}