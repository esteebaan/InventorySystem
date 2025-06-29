using InventorySystem.DataAccess;
using InventorySystem.Entities.Entities;

namespace InventorySystem.Business.Services
{
    /// <summary>
    /// Implementación de lógica de negocio para empleados.
    /// </summary>
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _uow;

        public EmployeeService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _uow.Employees.GetAllAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(Guid id)
        {
            return await _uow.Employees.GetByIdAsync(id);
        }

        public async Task<Guid> CreateEmployeeAsync(Employee employee)
        {
            employee.HireDate = DateTime.UtcNow;
            await _uow.Employees.AddAsync(employee);
            await _uow.CompleteAsync();
            return employee.EmployeeId;
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            _uow.Employees.Update(employee);
            await _uow.CompleteAsync();
        }

        public async Task DeleteEmployeeAsync(Guid id)
        {
            var existing = await _uow.Employees.GetByIdAsync(id);
            if (existing != null)
            {
                _uow.Employees.Remove(existing);
                await _uow.CompleteAsync();
            }
        }
    }
}
