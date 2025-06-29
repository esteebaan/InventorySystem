namespace InventorySystem.API.DTOs
{
    /// <summary>
    /// DTO para representar una observación.
    /// </summary>
    public class ObservationDto
    {
        public Guid ObservationId { get; set; }
        public Guid LoanId { get; set; }
        public string Text { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
