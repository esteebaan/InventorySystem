namespace InventorySystem.Entities.Entities
{
    public class Article
    {
        public Guid ArticleId { get; set; }     // Identificador único
        public string Code { get; set; } = null!;   // Código interno
        public string Name { get; set; } = null!;   // Descripción o nombre
        public string Category { get; set; } = null!; // Categoría del artículo
        public string Status { get; set; } = null!;   // Estado (e.g., Available, Loaned)
        public string Location { get; set; } = null!; // Ubicación física

        // Préstamos asociados a este artículo
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    }
}
