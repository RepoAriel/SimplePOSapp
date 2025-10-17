namespace SimplePOS.Business.DTOs
{
    /// <summary>
    /// Representa los datos de lectura de un producto.
    /// </summary>
    public class ProductReadDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }
}