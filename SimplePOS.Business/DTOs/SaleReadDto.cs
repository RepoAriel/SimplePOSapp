namespace SimplePOS.Business.DTOs
{
    public class SaleReadDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; } = new string(string.Empty);
        public string? UserId { get; set; }
        public List<SaleItemReadDto>? Items { get; set; }
    }
}