using InventorySystem.Entities.Entities;

namespace InventorySystem.Business.Services
{
    /// <summary>
    /// Interface que define operaciones de negocio sobre usuarios.
    /// </summary>
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(Guid id);
        Task<Guid> CreateUserAsync(User user); // Opcional si lo usás
        Task<Guid> CreateUserWithRoleAsync(string firstName, string lastName, string email, string passwordHash, string roleName);
        // ✅ nuevo
        Task UpdateUserAsync(Guid userId, string firstName, string lastName, string email, string roleName);
        // ✅ con rol
        Task DeleteUserAsync(Guid id);

    }
}
