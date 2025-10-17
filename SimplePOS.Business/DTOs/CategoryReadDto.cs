namespace SimplePOS.Business.DTOs
{
    /// <summary>
    /// Representa una categor�a de producto para lectura.
    /// </summary>
    public class CategoryReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}