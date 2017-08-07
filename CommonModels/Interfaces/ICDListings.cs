using CentralDispatchData.interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Interfaces
{
    public interface ICDListings
    {
        int Count { get; set; }
        int PageStart { get; set; }
        IList<ICDListing> Listings { get; set; }
    }
}
