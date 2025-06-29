namespace InventorySystem.API.DTOs
{
    /// <summary>
    /// DTO para actualizar datos de un cliente.
    /// </summary>
    public class UpdateClientDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
    }
}
