using InventorySystem.DataAccess.Repositories;
using InventorySystem.Entities.Entities;

namespace InventorySystem.DataAccess
{
    /// <summary>
    /// Implementación del Unit of Work para orquestar operaciones de repositorio.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IRepository<User> Users { get; }
        public IRepository<Role> Roles { get; }
        public IRepository<Employee> Employees { get; }
        public IRepository<Client> Clients { get; }
        public IRepository<Article> Articles { get; }
        public IRepository<Loan> Loans { get; }
        public IRepository<Observation> Observations { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Users = new Repository<User>(context);
            Roles = new Repository<Role>(context);
            Employees = new Repository<Employee>(context);
            Clients = new Repository<Client>(context);
            Articles = new Repository<Article>(context);
            Loans = new Repository<Loan>(context);
            Observations = new Repository<Observation>(context);
        }

        public async Task<int> CompleteAsync()
        {
            // Ejecuta SaveChanges en EF Core y devuelve el número de registros afectados
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            // Libera el DbContext
            _context.Dispose();
        }
    }
}
