using AssecorAssessment.Models;
using Microsoft.EntityFrameworkCore;

namespace AssecorAssessment.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("Persons");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Lastname).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Zipcode).HasMaxLength(10);
                entity.Property(p => p.City).HasMaxLength(100);
                entity.Property(p => p.Color).HasMaxLength(50);
            });
        }
    }
}