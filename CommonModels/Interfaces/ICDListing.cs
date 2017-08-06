using CentralDispatchData.Models;
using Common.Models;
using System;

namespace CentralDispatchData.interfaces
{
    public interface ICDListing : ICDListingMinimal
    {
        LonLat Pickup { get; set; }
        LonLat Delivery { get; set; }
        double Price { get; set; }
        double PricePerMile { get; set; }
        bool VehicleOperable { get; set; }
        TrailerType ShipMethod { get; set; }
        string VehicleTypes { get; set; }
        string TruckMiles { get; set; }
        bool LocationsValid { get; set; }
    }
}
