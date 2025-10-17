using System.ComponentModel.DataAnnotations;

namespace SimplePOS.Business.DTOs
{
    /// <summary>
    /// Representa los datos necesarios para actualizar un cliente existente.
    /// </summary>
    public class ClientUpdateDto
    {
        [Required(ErrorMessage = "El ID del cliente es obligatorio.")]
        public int Id { get; set; }

        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string? Name { get; set; }

        [StringLength(20, ErrorMessage = "El documento no puede superar los 20 caracteres.")]
        public string? Document { get; set; }

        [EmailAddress(ErrorMessage = "El correo electr�nico no es v�lido.")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "El n�mero de tel�fono no es v�lido.")]
        [StringLength(20, ErrorMessage = "El n�mero de tel�fono no puede superar los 20 caracteres.")]
        public string? PhoneNumber { get; set; }
    }
}