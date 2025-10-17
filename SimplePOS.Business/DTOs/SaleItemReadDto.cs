namespace SimplePOS.Business.DTOs
{
    /// <summary>
    /// Representa un artículo de venta con detalles como ID, producto, cantidad y precio unitario.
    /// </summary>
    public class SaleItemReadDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal => Quantity * UnitPrice;
        public string? ProductName { get; set; }
    }
}