using InventorySystem.Entities.Entities;

namespace InventorySystem.Business.Services
{
    /// <summary>
    /// Interface para la lógica de observaciones.
    /// </summary>
    public interface IObservationService
    {
        Task<IEnumerable<Observation>> GetObservationsByLoanAsync(Guid loanId);
        Task<Guid> AddObservationAsync(Observation observation);
    }
}
