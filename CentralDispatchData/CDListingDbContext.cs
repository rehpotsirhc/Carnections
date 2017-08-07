using CentralDispatchData.interfaces;
using Microsoft.EntityFrameworkCore;
namespace CentralDispatchData
{
    public class CDListingDbContext :  DbContext, ICDListingDbContext
    {
        public CDListingDbContext()
        {
        }
       // public DbSet<ICDListing> CDListings { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ICDListing>().ToTable("CDListing");
            modelBuilder.Entity<ICDListing>().HasKey(d => d.ListingId);
            //  modelBuilder.Entity<ICDListing>().Property(d => d.Id).ValueGeneratedOnAdd();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        }
    }
}
