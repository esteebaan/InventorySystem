namespace InventorySystem.API.DTOs
{
    /// <summary>
    /// DTO para crear una nueva observación.
    /// </summary>
    public class CreateObservationDto
    {
        public Guid LoanId { get; set; }
        public string Text { get; set; } = null!;
    }
}
