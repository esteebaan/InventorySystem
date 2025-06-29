using Microsoft.EntityFrameworkCore;

namespace InventorySystem.DataAccess.Repositories
{
    /// <summary>
    /// Implementación genérica del repositorio usando EF Core.
    /// </summary>
    /// <typeparam name="T">Entidad sobre la que opera el repositorio.</typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            // Devuelve todos los registros
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            // Busca por clave primaria
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            // Añade la entidad al Change Tracker
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            // Marca la entidad como modificada
            _dbSet.Update(entity);
        }

        public void Remove(T entity)
        {
            // Marca la entidad como eliminada
            _dbSet.Remove(entity);
        }
    }
}
