namespace InventorySystem.Entities.Entities
{
    public class Employee
    {
        public Guid EmployeeId { get; set; }   // Identificador único de empleado
        public Guid UserId { get; set; }       // FK hacia User
        public int RoleId { get; set; }        // FK hacia Role
        public DateTime HireDate { get; set; } // Fecha de ingreso

        // Propiedades de navegación
        public User User { get; set; } = null!;    // Usuario asociado
        public Role Role { get; set; } = null!;    // Rol asignado
        public ICollection<Loan> Loans { get; set; } = new List<Loan>(); // Préstamos realizados
    }
}
