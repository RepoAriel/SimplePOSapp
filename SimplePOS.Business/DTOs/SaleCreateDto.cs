namespace SimplePOS.Business.DTOs
{
    public class SaleCreateDto
    {
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public int ClientId { get; set; }
        public string? UserId { get; set; }
        public List<SaleItemCreateDto>? Items { get; set; }
    }
}