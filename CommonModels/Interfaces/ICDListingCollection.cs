using Common.Models;
using System.Collections.Generic;

namespace Common.Interfaces
{
    public interface ICDListingCollection
    {
        int Count { get; }
        int PageStart { get; }
        List<CDListing> Listings { get; }
    }
}
