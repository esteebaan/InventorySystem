using InventorySystem.Entities.Entities;

namespace InventorySystem.Business.Services
{
    /// <summary>
    /// Interface para la lógica de préstamos.
    /// </summary>
    public interface ILoanService
    {
        Task<IEnumerable<Loan>> GetAllLoansAsync();
        Task<Loan?> GetLoanByIdAsync(Guid id);
        Task<Guid> CreateLoanAsync(Loan loan);
        Task UpdateLoanAsync(Loan loan);
        Task DeleteLoanAsync(Guid id);
        Task UpdateLoanStatusAsync(Guid id, string newStatus);

    }
}
