using Common.Interfaces;
using System.Collections.Generic;

namespace CentralDispatchData.Repositories
{
    public interface ICDListingRepository
    {
        void DeleteOld(int daysOld, int commitCount, bool recreateContext);
        void AddorUpdate(IEnumerable<ITransformedListing> listings, int commitCount, bool recreateContext);
        IEnumerable<ITransformedListing> GetAll();
    }
}
