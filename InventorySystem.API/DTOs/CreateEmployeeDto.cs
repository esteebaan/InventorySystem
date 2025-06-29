namespace InventorySystem.API.DTOs
{
    /// <summary>
    /// DTO para crear un empleado.
    /// </summary>
    public class CreateEmployeeDto
    {
        public Guid UserId { get; set; }
        public int RoleId { get; set; }
        public DateTime HireDate { get; set; } // Puede omitirse y asignarse automáticamente en el servicio
    }
}
