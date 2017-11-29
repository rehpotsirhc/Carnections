using Common.Interfaces;
using Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CentralDispatchData
{
    public class CDListingDbContext :  DbContext
    {
        public CDListingDbContext(DbContextOptions options):base(options)
        {

        }
        public DbSet<TransformedListing> CDListings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TransformedListing>().ToTable("TransformedListing");
            modelBuilder.Entity<TransformedListing>().HasKey(d => d.ListingId);
            modelBuilder.Entity<TransformedListing>().Property(d => d.CreatedDate).ValueGeneratedOnAdd();
            base.OnModelCreating(modelBuilder);
        }
    }
}
