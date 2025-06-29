using InventorySystem.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DbSets para cada entidad
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Client> Clients { get; set; } = null!;
        public DbSet<Article> Articles { get; set; } = null!;
        public DbSet<Loan> Loans { get; set; } = null!;
        public DbSet<Observation> Observations { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de la relación uno a uno User-Employee
            modelBuilder.Entity<User>()
                .HasOne(u => u.Employee)
                .WithOne(e => e.User)
                .HasForeignKey<Employee>(e => e.UserId);

            // Configuración de la relación uno a muchos Role-Employee
            modelBuilder.Entity<Role>()
                .HasMany(r => r.Employees)
                .WithOne(e => e.Role)
                .HasForeignKey(e => e.RoleId);

            // Configuración de la relación uno a muchos Employee-Loan
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Loans)
                .WithOne(l => l.Employee)
                .HasForeignKey(l => l.EmployeeId);

            // Configuración de la relación uno a muchos Client-Loan
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Loans)
                .WithOne(l => l.Client)
                .HasForeignKey(l => l.ClientId);

            // Configuración de la relación uno a muchos Article-Loan
            modelBuilder.Entity<Article>()
                .HasMany(a => a.Loans)
                .WithOne(l => l.Article)
                .HasForeignKey(l => l.ArticleId);

            // Configuración de la relación uno a muchos Loan-Observation
            modelBuilder.Entity<Observation>()
                .HasOne(o => o.Loan)
                .WithMany(l => l.Observations)
                .HasForeignKey(o => o.LoanId);

        }


    }
}
