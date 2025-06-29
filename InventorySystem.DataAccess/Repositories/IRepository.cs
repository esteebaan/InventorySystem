namespace InventorySystem.DataAccess.Repositories
{
    /// <summary>
    /// Interfaz genérica para operaciones CRUD comunes.
    /// </summary>
    /// <typeparam name="T">Entidad sobre la que opera el repositorio.</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Obtiene todos los registros de la entidad.
        /// </summary>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Obtiene un registro por su identificador.
        /// </summary>
        /// <param name="id">Identificador único.</param>
        Task<T?> GetByIdAsync(Guid id);

        /// <summary>
        /// Agrega una nueva entidad al contexto.
        /// </summary>
        /// <param name="entity">Instancia de la entidad.</param>
        Task AddAsync(T entity);

        /// <summary>
        /// Actualiza una entidad existente.
        /// </summary>
        /// <param name="entity">Instancia de la entidad.</param>
        void Update(T entity);

        /// <summary>
        /// Elimina una entidad del contexto.
        /// </summary>
        /// <param name="entity">Instancia de la entidad.</param>
        void Remove(T entity);
    }
}
