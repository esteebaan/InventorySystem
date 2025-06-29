namespace InventorySystem.Entities.Entities
{
    public class Client
    {
        public Guid ClientId { get; set; }         // Identificador único de cliente
        public string FirstName { get; set; } = null!; // Nombre de pila
        public string LastName { get; set; } = null!;  // Apellido
        public string Email { get; set; } = null!;     // Correo electrónico
        public string Phone { get; set; } = null!;     // Teléfono de contacto

        // Préstamos que este cliente ha solicitado
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    }
}
