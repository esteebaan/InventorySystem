namespace InventorySystem.Entities.Entities
{
    public class Role
    {
        public int RoleId { get; set; }        // PK autoincremental
        public string Name { get; set; } = null!; // Nombre del rol (e.g., Admin, Clerk)

        // Colección de empleados asignados a este rol
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
