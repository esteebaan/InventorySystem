namespace InventorySystem.Entities.Entities
{
    public class Loan
    {
        public Guid LoanId { get; set; }           // Identificador único de préstamo
        public Guid ArticleId { get; set; }        // FK hacia Article
        public Guid EmployeeId { get; set; }       // FK hacia Employee
        public Guid ClientId { get; set; }         // FK hacia Client
        public DateTime RequestedAt { get; set; }  // Fecha de solicitud
        public DateTime DeliveredAt { get; set; }  // Fecha de entrega
        public DateTime? ReturnedAt { get; set; }  // Fecha de devolución (opcional)
        public string Status { get; set; } = null!;   // Estado (e.g., Pending, Completed)

        // Navegación
        public Article Article { get; set; } = null!;
        public Employee Employee { get; set; } = null!;
        public Client Client { get; set; } = null!;
        public ICollection<Observation> Observations { get; set; } = new List<Observation>();
    }
}
