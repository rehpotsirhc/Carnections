using System;
using System.Collections.Generic;
using System.Text;

namespace CentralDispatchData.Models
{
    public interface ICDListingMinimal
    {
        //primary key of CDListing
        int ListingId { get; set; }
        DateTime ModifiedDate { get; set; }

    }
}
