using Common.Interfaces;
using System;
using System.Collections.Generic;

namespace Common.Models
{
    public class TransformedListing : CDListing, ITransformedListing
    {
        public int Id { get { return ListingId; } }
        public TrailerTypeWeight TrailerTypeWeight { get; set;  }
        public List<VehicleTypeSize> VehicleTypesSizes { get; set; }
        public double AverageVehicleWeight { get; set; }
        public int VehicleCount { get; set; }
        public int MilesInterpolated { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
