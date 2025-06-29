namespace InventorySystem.API.DTOs
{
    /// <summary>
    /// DTO para representar un cliente.
    /// </summary>
    public class ClientDto
    {
        public Guid ClientId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
    }
}
