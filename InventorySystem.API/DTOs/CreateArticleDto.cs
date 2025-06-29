namespace InventorySystem.API.DTOs
{
    /// <summary>
    /// DTO para crear un artículo.
    /// </summary>
    public class CreateArticleDto
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string Location { get; set; } = null!;
    }
}
