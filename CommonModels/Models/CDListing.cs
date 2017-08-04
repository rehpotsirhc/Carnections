using CentralDispatchData.interfaces;
using System.Collections.Generic;
using Common.Models;
using Newtonsoft.Json;

namespace CentralDispatchData.Models
{
    public class CDListings
    {
        public int Count { get; set; }
        public int PageStart { get; set; }
        public IList<CDListing> Listings { get; set; }
    }
    public class CDListing : ICDListing
    {
        //pk 
        public int Id { get; set; }
        public LonLat Pickup { get; set; }
        public LonLat Delivery { get; set; }
        public int ListingId { get; set; }
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
