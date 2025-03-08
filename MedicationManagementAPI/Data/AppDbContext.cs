using Microsoft.EntityFrameworkCore;
using MedicationManagementAPI.Models;

namespace MedicationManagementAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Medication> Medications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Medications)
                .WithOne(m => m.User)
                .HasForeignKey(m => m.UserId);
        }
    }
}
