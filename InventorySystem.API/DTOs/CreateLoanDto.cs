namespace InventorySystem.API.DTOs
{
    /// <summary>
    /// DTO para crear un préstamo.
    /// </summary>
    public class CreateLoanDto
    {
        public Guid ArticleId { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid ClientId { get; set; }
        public DateTime DeliveredAt { get; set; }
        public string Status { get; set; } = null!; // Estado inicial (e.g., \"Pending\")
    }
}
