namespace InventorySystem.Entities.Entities
{
    public class Observation
    {
        public Guid ObservationId { get; set; }    // Identificador único
        public Guid LoanId { get; set; }           // FK hacia Loan
        public string Text { get; set; } = null!;  // Texto de la observación
        public DateTime CreatedAt { get; set; }    // Fecha de creación

        // Navegación
        public Loan Loan { get; set; } = null!;
    }
}
