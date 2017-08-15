using Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace VehicleData
{

    public class VehicleDbContext : DbContext
    {
        public VehicleDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IVehicleMinimal>().ToTable("Vehicle");
            modelBuilder.Entity<IVehicleMinimal>().HasKey(d => d.Id);
            modelBuilder.Entity<IVehicleMinimal>().Property(d => d.Id).ValueGeneratedOnAdd();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}
