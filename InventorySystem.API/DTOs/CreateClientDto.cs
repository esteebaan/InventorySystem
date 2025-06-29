namespace InventorySystem.API.DTOs
{
    /// <summary>
    /// DTO para crear un cliente.
    /// </summary>
    public class CreateClientDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
    }
}
