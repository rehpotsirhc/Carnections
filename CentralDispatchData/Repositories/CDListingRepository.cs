using Common.Interfaces;
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

            var dbSet = _dbContext.Set<IHasListingIdAndChangeDates>();
            IQueryable<IHasListingIdAndChangeDates> listings = dbSet.Where(listing => listing.ModifiedDate < DateTime.Now.AddDays(-daysOld));

            BulkDatbaseOperation(listings, commitCount, recreateContext, DeleteToContext);
        }

        public void AddorUpdate(IEnumerable<ITransformedListing> listings, int commitCount = DEFAULT_COMMIT_COUNT, bool recreateContext = DEFAULT_RECREATE_CONTEXT)
        {
            //listings must be the full ICDListing and not ICDListingMinimal
            BulkDatbaseOperation(listings, commitCount, recreateContext, AddorUpdateToContext);
        }
        public IEnumerable<ITransformedListing> GetAll()
        {
            if (_dbContext == null)
                _dbContext = new CDListingDbContext();

            return _dbContext.Set<ITransformedListing>();
        }

        private async void BulkDatbaseOperation(IEnumerable<IHasListingIdAndChangeDates> listings, int commitCount, bool recreateContext,
            Action<IHasListingIdAndChangeDates, CDListingDbContext> dataBaseOperation)
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

        private static async void AddorUpdateToContext(IHasListingIdAndChangeDates listing, CDListingDbContext context)
        {
            listing.ModifiedDate = DateTime.UtcNow;
            var dbSetForFind = context.Set<IHasListingIdAndChangeDates>();
            var foundListing = await dbSetForFind.FindAsync(listing.ListingId);

            var dbSetFull = context.Set<ITransformedListing>();

            if (foundListing == null || foundListing.ListingId < 0)
                dbSetFull.Add((ITransformedListing)listing);
            else dbSetFull.Update((ITransformedListing)listing);
        }
        private static void DeleteToContext(IHasListingIdAndChangeDates listing, CDListingDbContext context)
        {
            var dbSet = context.Set<IHasListingIdAndChangeDates>();
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
