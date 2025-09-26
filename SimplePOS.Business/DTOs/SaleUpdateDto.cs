namespace SimplePOS.Business.DTOs
{
    public class SaleUpdateDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public int ClientId { get; set; }
        public string? UserId { get; set; }
        public List<SaleItemUpdateDto>? Items { get; set; }
    }
}