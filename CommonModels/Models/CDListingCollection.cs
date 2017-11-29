﻿using System.Collections.Generic;
using Common.Interfaces;

namespace Common.Models
{
    public class CDListingCollection : ICDListingCollection
    {
        public int Count { get; set; }
        public int PageStart { get; set; }
        public List<CDListing> Listings { get; set; }
    }
}
