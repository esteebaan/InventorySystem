namespace InventorySystem.API.DTOs
{
    /// <summary>
    /// DTO para actualizar un usuario existente.
    /// </summary>
    public class UpdateUserDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
