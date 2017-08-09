using Common.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace CentralDispatchData
{
    public class CDListingDbContext :  DbContext
    {
        public CDListingDbContext()
        {
        }
       // public DbSet<ICDListing> CDListings { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ITransformedListing>().ToTable("TransformedListing");
            modelBuilder.Entity<ITransformedListing>().HasKey(d => d.ListingId);
            //  modelBuilder.Entity<ICDListing>().Property(d => d.Id).ValueGeneratedOnAdd();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        }
    }
}
