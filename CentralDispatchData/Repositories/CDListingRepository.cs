using Common.Interfaces;
using Common.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentralDispatchData.Repositories
{
    public class CDListingRepository : ICDListingRepository
    {
        private const int DEFAULT_COMMIT_COUNT = -1;
        private const bool DEFAULT_RECREATE_CONTEXT = false;
        private CDListingDbContext _dbContext;
        private static int numberUpdated = 0;
        private static int numberAdded = 0;
        public CDListingRepository(CDListingDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbContext.CDListings = _dbContext.Set<TransformedListing>();
        }


        public int DeleteOld(int daysOld, int commitCount = DEFAULT_COMMIT_COUNT, bool disposeContextWhenDone = true)
        {
            IQueryable<TransformedListing> listings = _dbContext.CDListings.Where(listing => listing.ModifiedDate < DateTime.Now.AddDays(-daysOld));

            return BulkDatbaseOperation(listings, commitCount, DeleteToContext);
        }

        public int AddorUpdate(IEnumerable<TransformedListing> listings, int commitCount = DEFAULT_COMMIT_COUNT, bool disposeContextWhenDone = true)
        {
            Console.WriteLine("In add or update");
            //listings must be the full ICDListing and not ICDListingMinimal
            return BulkDatbaseOperation(listings, commitCount, AddorUpdateToContext);
        }
        public IEnumerable<TransformedListing> GetAll()
        {
            return _dbContext.CDListings;
        }

        private int BulkDatbaseOperation(IEnumerable<TransformedListing> listings, int commitCount,
            Action<TransformedListing, CDListingDbContext> dataBaseOperation, bool disposeContextWhenDone = true)
        {
            if (commitCount == DEFAULT_COMMIT_COUNT)
                commitCount = listings.Count();

            try
            {
                int count = 0;
                if (listings != null & listings.Count() > 0)
                {
                    foreach (var listing in listings)
                    {

                        dataBaseOperation(listing, _dbContext);

                        if (++count % commitCount == 0)
                            _dbContext.SaveChanges();
                    }
                }
                _dbContext.SaveChanges();
                Console.WriteLine($"Listings processed: {count}");
                return count;
            }
            catch (Exception e)
            {
                //log exception
                Console.WriteLine(e);
                return -1;
            }
            finally
            {
                if (disposeContextWhenDone)
                {
                    _dbContext.Dispose();
                    Console.WriteLine("Context disposed");
                    Console.WriteLine("");
                }
            }
        }

        private static void AddorUpdateToContext(TransformedListing listing, CDListingDbContext context)
        {
            listing.ModifiedDate = DateTime.UtcNow;
            var foundListing = context.CDListings.Find(listing.ListingId);

            if (foundListing == null || foundListing.ListingId < 0)
            {
                Console.WriteLine("Listing added");
                context.CDListings.Add(listing);
             //   Console.WriteLine($"Number Added: {++numberAdded}");
            }
            else
            {
                context.CDListings.Update(listing);
              Console.WriteLine($"Number Updated: {++numberUpdated}");
            }
        }

        private static void DeleteToContext(TransformedListing listing, CDListingDbContext context)
        {
            context.CDListings.Remove(listing);
        }
    }
}
