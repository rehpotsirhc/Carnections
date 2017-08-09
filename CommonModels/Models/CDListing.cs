using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Enums.Models;
using Newtonsoft.Json;

namespace Common.Models
{
    public class CDListing : ICDListing
    {
        public int ListingId { get; set; }
        public ILonLat Pickup { get; set; }
        public ILonLat Delivery { get; set; }
        public double Price { get; set; }
        public double PricePerMile { get; set; }
        public bool VehicleOperable { get; set; }
        public ETrailerType ShipMethod { get; set; }
        [JsonProperty("vehicle_types")]
        public string VehicleTypes { get; set; }
        public string TruckMiles { get; set; }
        [JsonProperty("locations_valid")]
        public bool LocationsValid { get; set; }

    }
}
