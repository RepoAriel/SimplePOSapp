namespace SimplePOS.Business.DTOs
{
    /// <summary>
    /// Representa una categoría de producto para lectura.
    /// </summary>
    public class CategoryReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}