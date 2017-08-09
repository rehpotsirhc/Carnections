using System.Collections.Generic;


namespace Common.Interfaces
{
    public interface ICDListingCollection
    {
        int Count { get; set; }
        int PageStart { get; set; }
        IList<ICDListing> Listings { get; set; }
    }
}
