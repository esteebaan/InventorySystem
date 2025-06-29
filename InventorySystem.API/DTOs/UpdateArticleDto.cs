namespace InventorySystem.API.DTOs
{
    /// <summary>
    /// DTO para actualizar un artículo existente.
    /// </summary>
    public class UpdateArticleDto
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string Location { get; set; } = null!;
    }
}
