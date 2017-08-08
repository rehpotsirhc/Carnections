﻿using CentralDispatchData.interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CentralDispatchData.Repositories
{
    public interface ICDListingRepository
    {
        void AddorUpdate(IEnumerable<ICDListing> listings, int commitCount, bool recreateContext);
        IEnumerable<ICDListing> GetAll();
    }
}
