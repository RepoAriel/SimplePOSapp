namespace SimplePOS.Business.DTOs
{
    /// <summary>
    /// Representa una venta con detalles como ID, fecha, total, cliente e ítems de venta.
    /// </summary>
    public class SaleReadDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; } = new string(string.Empty);
        public List<SaleItemReadDto>? Items { get; set; }
    }
}