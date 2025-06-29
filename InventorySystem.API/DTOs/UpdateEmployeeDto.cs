namespace InventorySystem.API.DTOs
{
    /// <summary>
    /// DTO para actualizar un empleado existente.
    /// </summary>
    public class UpdateEmployeeDto
    {
        public int RoleId { get; set; }
        public DateTime HireDate { get; set; }
    }
}
