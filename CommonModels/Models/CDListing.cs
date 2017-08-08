using CentralDispatchData.interfaces;
using System.Collections.Generic;
using Common.Models;
using Newtonsoft.Json;
using System;
using Common.Interfaces;

namespace CentralDispatchData.Models
{
    public class CDListings : ICDListings
    {
        public int Count { get; set; }
        public int PageStart { get; set; }
        public IList<ICDListing> Listings { get; set; }
    }
    public class CDListing : CDListingMinimal, ICDListing, ICDListingMinimal
    {

        public int Id { get { return ListingId; } }
        public ILonLat Pickup { get; set; }
        public ILonLat Delivery { get; set; }
        public double Price { get; set; }
        public double PricePerMile { get; set; }
        public bool VehicleOperable { get; set; }
        public TrailerType ShipMethod { get; set; }
        [JsonProperty("vehicle_types")]
        public string VehicleTypes { get; set; }
        public string TruckMiles { get; set; }
        [JsonProperty("locations_valid")]
        public bool LocationsValid { get; set; }

    }
}
