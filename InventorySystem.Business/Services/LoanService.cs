using InventorySystem.DataAccess;
using InventorySystem.Entities.Entities;

namespace InventorySystem.Business.Services
{
    /// <summary>
    /// Implementación de lógica de negocio para préstamos.
    /// </summary>
    public class LoanService : ILoanService
    {
        private readonly IUnitOfWork _uow;

        public LoanService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<Loan>> GetAllLoansAsync()
        {
            return await _uow.Loans.GetAllAsync();
        }

        public async Task<Loan?> GetLoanByIdAsync(Guid id)
        {
            return await _uow.Loans.GetByIdAsync(id);
        }

        public async Task<Guid> CreateLoanAsync(Loan loan)
        {
            loan.RequestedAt = DateTime.UtcNow;
            // Aquí podríamos hacer validaciones (disponibilidad del artículo, etc.)
            await _uow.Loans.AddAsync(loan);
            await _uow.CompleteAsync();
            return loan.LoanId;
        }

        public async Task UpdateLoanAsync(Loan loan)
        {
            _uow.Loans.Update(loan);
            await _uow.CompleteAsync();
        }

        public async Task DeleteLoanAsync(Guid id)
        {
            var existing = await _uow.Loans.GetByIdAsync(id);
            if (existing != null)
            {
                _uow.Loans.Remove(existing);
                await _uow.CompleteAsync();
            }
        }

        public async Task UpdateLoanStatusAsync(Guid id, string newStatus)
        {
            var loans = await _uow.Loans.GetAllAsync();
            var loan = loans.FirstOrDefault(l => l.LoanId == id);
            if (loan == null)
                throw new InvalidOperationException("Loan not found.");

            if (loan.Status == "Returned")
                throw new InvalidOperationException("Cannot change status of a returned loan.");

            loan.Status = newStatus;

            _uow.Loans.Update(loan);
            await _uow.CompleteAsync();
        }

    }
}
