using InventorySystem.DataAccess.Repositories;
using InventorySystem.Entities.Entities;

namespace InventorySystem.DataAccess
{
    /// <summary>
    /// Interface para coordinar múltiples repositorios y transacciones.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        IRepository<Role> Roles { get; }
        IRepository<Employee> Employees { get; }
        IRepository<Client> Clients { get; }
        IRepository<Article> Articles { get; }
        IRepository<Loan> Loans { get; }
        IRepository<Observation> Observations { get; }

        /// <summary>
        /// Persiste todos los cambios pendientes en la base de datos.
        /// </summary>
        Task<int> CompleteAsync();
    }
}
