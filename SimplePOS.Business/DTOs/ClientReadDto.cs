namespace SimplePOS.Business.DTOs
{
    public class ClientReadDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Document { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}