namespace InventorySystem.API.DTOs
{
    /// <summary>
    /// DTO para representar un préstamo.
    /// </summary>
    public class LoanDto
    {
        public Guid LoanId { get; set; }
        public Guid ArticleId { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid ClientId { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime DeliveredAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
        public string Status { get; set; } = null!;
    }
}
