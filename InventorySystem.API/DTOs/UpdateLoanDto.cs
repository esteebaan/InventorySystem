namespace InventorySystem.API.DTOs
{
    /// <summary>
    /// DTO para actualizar un préstamo existente.
    /// </summary>
    public class UpdateLoanDto
    {
        public DateTime? ReturnedAt { get; set; }  // Fecha de devolución
        public string Status { get; set; } = null!; // Nuevo estado
    }
}
