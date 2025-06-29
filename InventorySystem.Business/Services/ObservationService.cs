using InventorySystem.DataAccess;
using InventorySystem.Entities.Entities;

namespace InventorySystem.Business.Services
{
    /// <summary>
    /// Implementación de reglas de negocio para observaciones.
    /// </summary>
    public class ObservationService : IObservationService
    {
        private readonly IUnitOfWork _uow;

        public ObservationService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<Observation>> GetObservationsByLoanAsync(Guid loanId)
        {
            // Podríamos filtrar directamente en la consulta
            var all = await _uow.Observations.GetAllAsync();
            return all.Where(o => o.LoanId == loanId);
        }

        public async Task<Guid> AddObservationAsync(Observation observation)
        {
            observation.CreatedAt = DateTime.UtcNow;
            await _uow.Observations.AddAsync(observation);
            await _uow.CompleteAsync();
            return observation.ObservationId;
        }
    }
}
