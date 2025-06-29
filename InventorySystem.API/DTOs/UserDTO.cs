namespace InventorySystem.API.DTOs
{
    /// <summary>
    /// DTO para representar un usuario en peticiones/respuestas.
    /// </summary>
    public class UserDTO
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string? Role { get; set; } // ⚠️ Esto debe estar presente
    }
}
