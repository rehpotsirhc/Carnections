using CentralDispatchData.interfaces;
using CentralDispatchData.Models;
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

        CDListingDbContext _dbContext;

        public CDListingRepository(CDListingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void DeleteOld(int daysOld, int commitCount = DEFAULT_COMMIT_COUNT, bool recreateContext = DEFAULT_RECREATE_CONTEXT)
        {
            if (_dbContext == null || recreateContext)
                _dbContext = new CDListingDbContext();

            var dbSet = _dbContext.Set<ICDListingMinimal>();
            IQueryable<ICDListingMinimal> listings = dbSet.Where(listing => listing.ModifiedDate < DateTime.Now.AddDays(-daysOld));

            BulkDatbaseOperation(listings, commitCount, recreateContext, DeleteToContext);
        }

        public void AddorUpdate(IEnumerable<ICDListing> listings, int commitCount = DEFAULT_COMMIT_COUNT, bool recreateContext = DEFAULT_RECREATE_CONTEXT)
        {
            //listings must be the full ICDListing and not ICDListingMinimal
            BulkDatbaseOperation(listings, commitCount, recreateContext, AddorUpdateToContext);
        }
        public IEnumerable<ICDListing> GetAll()
        {
            if (_dbContext == null)
                _dbContext = new CDListingDbContext();

           return _dbContext.Set<ICDListing>();
        }

        private async void BulkDatbaseOperation(IEnumerable<ICDListingMinimal> listings, int commitCount, bool recreateContext,
            Action<ICDListingMinimal, CDListingDbContext> dataBaseOperation)
        {
            if (commitCount == DEFAULT_COMMIT_COUNT)
                commitCount = listings.Count();

            try
            {
                if (_dbContext == null || recreateContext)
                    _dbContext = new CDListingDbContext();

                int count = 0;
                foreach (var listing in listings)
                {

                    dataBaseOperation(listing, _dbContext);
                    _dbContext = await SaveAndFlush(_dbContext, ++count, commitCount, recreateContext);
                }

               await _dbContext.SaveChangesAsync();
            }
            finally
            {
                if (_dbContext != null)
                    _dbContext.Dispose();
            }

        }

        private static async void AddorUpdateToContext(ICDListingMinimal listing, CDListingDbContext context)
        {
            listing.ModifiedDate = DateTime.UtcNow;
            var dbSetForFind = context.Set<ICDListingMinimal>();
            var foundListing = await dbSetForFind.FindAsync(listing.ListingId);

            var dbSetFull = context.Set<ICDListing>();

            if (foundListing == null || foundListing.ListingId < 0)
                await dbSetFull.AddAsync((ICDListing)listing);
            else dbSetFull.Update((ICDListing)listing);
        }
        private static void DeleteToContext(ICDListingMinimal listing, CDListingDbContext context)
        {
            var dbSet = context.Set<ICDListingMinimal>();
            dbSet.Remove(listing);
        }

        private static async Task<CDListingDbContext> SaveAndFlush(CDListingDbContext context, int count, int commitCount, bool recreateContext)
        {
            if (count % commitCount == 0)
            {
                await context.SaveChangesAsync();
                if (recreateContext)
                {
                    context.Dispose();
                    context = new CDListingDbContext();
                }
            }
            return context;
        }
    }
}
