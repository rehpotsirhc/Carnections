using CentralDispatchData.interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Common.Models;

namespace CentralDispatchData.Models
{
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
        public string VehicleTypes { get; set; }
        public string TruckMiles { get; set; }
        public bool LocationsValid { get; set; }
    }
}
