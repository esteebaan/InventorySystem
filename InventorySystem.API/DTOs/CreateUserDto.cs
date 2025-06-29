namespace InventorySystem.API.DTOs
{
    /// <summary>
    /// DTO para crear un nuevo usuario.
    /// </summary>
    public class CreateUserDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!; // Plain text, se convertirá a hash
        public string Role { get; set; } = null!;
    }
}
