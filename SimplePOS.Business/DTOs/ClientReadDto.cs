namespace SimplePOS.Business.DTOs
{
    /// <summary>
    /// Representa los datos de un cliente para lectura.
    /// </summary>
    public class ClientReadDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Document { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}