using InventorySystem.DataAccess;
using InventorySystem.Entities.Entities;

namespace InventorySystem.Business.Services
{
    /// <summary>
    /// Implementación de lógica de negocio para clientes.
    /// </summary>
    public class ClientService : IClientService
    {
        private readonly IUnitOfWork _uow;

        public ClientService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<Client>> GetAllClientsAsync()
        {
            return await _uow.Clients.GetAllAsync();
        }

        public async Task<Client?> GetClientByIdAsync(Guid id)
        {
            return await _uow.Clients.GetByIdAsync(id);
        }

        public async Task<Guid> CreateClientAsync(Client client)
        {
            var all = await _uow.Clients.GetAllAsync();
            if (all.Any(c => c.Email == client.Email))
                throw new InvalidOperationException("Client email already exists.");

            await _uow.Clients.AddAsync(client);
            await _uow.CompleteAsync();
            return client.ClientId;
        }

        public async Task UpdateClientAsync(Client client)
        {
            _uow.Clients.Update(client);
            await _uow.CompleteAsync();
        }

        public async Task DeleteClientAsync(Guid id)
        {
            var existing = await _uow.Clients.GetByIdAsync(id);
            if (existing != null)
            {
                _uow.Clients.Remove(existing);
                await _uow.CompleteAsync();
            }
        }
    }
}
