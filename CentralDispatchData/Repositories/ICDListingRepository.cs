using Common.Interfaces;
using Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CentralDispatchData.Repositories
{
    public interface ICDListingRepository
    {
        int DeleteOld(int daysOld, int commitCount, bool disposeContextWhenDone = true);
        int AddorUpdate(IEnumerable<TransformedListing> listings, int commitCount, bool disposeContextWhenDone = true);
        IEnumerable<TransformedListing> GetAll();
    }
}
