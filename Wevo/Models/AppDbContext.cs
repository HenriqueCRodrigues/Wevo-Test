using Microsoft.EntityFrameworkCore;

namespace Wevo.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(u => u.name).IsRequired().HasMaxLength(80);

            modelBuilder.Entity<User>().Property(u => u.cpf).IsRequired().HasMaxLength(14);
            modelBuilder.Entity<User>().HasIndex(u => u.cpf).IsUnique();

            modelBuilder.Entity<User>().Property(u => u.email).IsRequired();
            modelBuilder.Entity<User>().HasIndex(u => u.email).IsUnique();
            
            modelBuilder.Entity<User>().Property(u => u.telephone).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.sex).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.birthday).IsRequired();
            
        }

    }
}
