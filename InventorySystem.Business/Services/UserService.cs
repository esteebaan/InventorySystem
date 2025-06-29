using InventorySystem.DataAccess;
using InventorySystem.Entities.Entities;


namespace InventorySystem.Business.Services
{
    /// <summary>
    /// Implementación de reglas de negocio para usuarios.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;

        public UserService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            // Aquí podríamos añadir paginación, filtros, etc.
            return await _uow.Users.GetAllAsync();

        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _uow.Users.GetByIdAsync(id);
        }

        public async Task<Guid> CreateUserAsync(User user)
        {
            // Validar datos, e.g. email único
            var allUsers = await _uow.Users.GetAllAsync();

            if (allUsers.Any(u => u.Email == user.Email))
                throw new InvalidOperationException("Email already exists.");

            user.CreatedAt = DateTime.UtcNow;

            await _uow.Users.AddAsync(user);
            await _uow.CompleteAsync();
            return user.UserId;
        }

        public async Task UpdateUserAsync(Guid userId, string firstName, string lastName, string email, string roleName)
        {
            var user = await _uow.Users.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            user.FirstName = firstName;
            user.LastName = lastName;
            user.Email = email;

            var employees = await _uow.Employees.GetAllAsync();
            var employee = employees.FirstOrDefault(e => e.UserId == userId);

            var roles = await _uow.Roles.GetAllAsync();
            var role = roles.FirstOrDefault(r => r.Name == roleName);
            if (role == null)
                throw new InvalidOperationException("Role not found.");

            if (employee != null)
            {
                employee.RoleId = role.RoleId;
                _uow.Employees.Update(employee);
            }
            else
            {
                var newEmployee = new Employee
                {
                    EmployeeId = Guid.NewGuid(),
                    UserId = userId,
                    RoleId = role.RoleId,
                    HireDate = DateTime.UtcNow
                };
                await _uow.Employees.AddAsync(newEmployee);
            }

            _uow.Users.Update(user);
            await _uow.CompleteAsync();
        }


        public async Task<Guid> CreateUserWithRoleAsync(string firstName, string lastName, string email, string passwordHash, string roleName)
        {
            var existingUsers = await _uow.Users.GetAllAsync();
            if (existingUsers.Any(u => u.Email == email))
                throw new InvalidOperationException("Email already exists.");

            var roles = await _uow.Roles.GetAllAsync();
            var role = roles.FirstOrDefault(r => r.Name == roleName);
            if (role == null)
                throw new InvalidOperationException("Role not found.");

            var user = new User
            {
                UserId = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow
            };

            var employee = new Employee
            {
                EmployeeId = Guid.NewGuid(),
                UserId = user.UserId,
                RoleId = role.RoleId,
                HireDate = DateTime.UtcNow
            };

            await _uow.Users.AddAsync(user);
            await _uow.Employees.AddAsync(employee);
            await _uow.CompleteAsync();

            return user.UserId;
        }


        public async Task DeleteUserAsync(Guid id)
        {
            var existing = await _uow.Users.GetByIdAsync(id);
            if (existing != null)
            {
                _uow.Users.Remove(existing);
                await _uow.CompleteAsync();
            }
        }

    }
}
