namespace InventorySystem.Entities.Entities
{
    public class User
    {
        public Guid UserId { get; set; }               // Identificador único
        public string FirstName { get; set; } = null!; // Nombre de pila
        public string LastName { get; set; } = null!;  // Apellido
        public string Email { get; set; } = null!;     // Correo electrónico
        public string PasswordHash { get; set; } = null!; // Hash de la contraseña
        public DateTime CreatedAt { get; set; }        // Fecha de alta

        // Relación uno a uno con Employee (si es empleado)
        public Employee? Employee { get; set; }
       
    }
}
