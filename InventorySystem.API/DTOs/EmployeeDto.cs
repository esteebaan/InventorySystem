namespace InventorySystem.API.DTOs
{
    /// <summary>
    /// DTO para representar un empleado.
    /// </summary>
    public class EmployeeDto
    {
        public Guid EmployeeId { get; set; }    // Identificador de empleado
        public Guid UserId { get; set; }    // FK al usuario
        public int RoleId { get; set; }    // FK al rol
        public DateTime HireDate { get; set; }  // Fecha de ingreso
    }
}
