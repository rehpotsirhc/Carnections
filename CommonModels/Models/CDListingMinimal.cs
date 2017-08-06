using CentralDispatchData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class CDListingMinimal : ICDListingMinimal
    {
        //primary key of CDListing
        public int ListingId { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
