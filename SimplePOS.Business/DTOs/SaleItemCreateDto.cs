namespace SimplePOS.Business.DTOs
{
    public class SaleItemCreateDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}