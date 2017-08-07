using CentralDispatchData.interfaces;
using CentralDispatchData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
namespace CentralDispatchData.Repositories
{
    public class CDListingRepository
    {
        CDListingDbContext _dbContext;

        public CDListingRepository(CDListingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void DeleteOld(int daysOld, int commitCount, bool recreateContext)
        {
            if (_dbContext == null || recreateContext)
                _dbContext = new CDListingDbContext();

            var dbSet = _dbContext.Set<ICDListingMinimal>();
            IQueryable<ICDListingMinimal> listings = dbSet.Where(listing => listing.ModifiedDate < DateTime.Now.AddDays(-daysOld));

            BulkDatbaseOperation(listings, commitCount, recreateContext, DeleteToContext);
        }

        public void AddorUpdate(IEnumerable<ICDListing> listings, int commitCount, bool recreateContext)
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

        private void BulkDatbaseOperation(IEnumerable<ICDListingMinimal> listings, int commitCount, bool recreateContext,
            Func<ICDListingMinimal, CDListingDbContext, CDListingDbContext> dataBaseOperation)
        {
            try
            {
                if (_dbContext == null || recreateContext)
                    _dbContext = new CDListingDbContext();

                int count = 0;
                foreach (var listing in listings)
                {

                    _dbContext = dataBaseOperation(listing, _dbContext);
                    _dbContext = SaveAndFlush(_dbContext, ++count, commitCount, recreateContext);
                }

                _dbContext.SaveChanges();
            }
            finally
            {
                if (_dbContext != null)
                    _dbContext.Dispose();
            }

        }

        private static CDListingDbContext AddorUpdateToContext(ICDListingMinimal listing, CDListingDbContext context)
        {
            listing.ModifiedDate = DateTime.UtcNow;
            var dbSetForFind = context.Set<ICDListingMinimal>();
            var foundListing = dbSetForFind.Find(listing.ListingId);

            var dbSetFull = context.Set<ICDListing>();

            if (foundListing == null || foundListing.ListingId < 0)
                dbSetFull.Add((ICDListing)listing);
            else dbSetFull.Update((ICDListing)listing);

            return context;
        }
        private static CDListingDbContext DeleteToContext(ICDListingMinimal listing, CDListingDbContext context)
        {
            var dbSet = context.Set<ICDListingMinimal>();
            dbSet.Remove(listing);
            return context;
        }

        private static CDListingDbContext SaveAndFlush(CDListingDbContext context, int count, int commitCount, bool recreateContext)
        {
            if (count % commitCount == 0)
            {
                context.SaveChanges();
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
