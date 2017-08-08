using CentralDispatchData.Models;
using Common.Interfaces;
using Common.Models;
using System;

namespace CentralDispatchData.interfaces
{
    public interface ICDListing : ICDListingMinimal, IHasId
    {
        ILonLat Pickup { get; set; }
        ILonLat Delivery { get; set; }
        double Price { get; set; }
        double PricePerMile { get; set; }
        bool VehicleOperable { get; set; }
        TrailerType ShipMethod { get; set; }
        string VehicleTypes { get; set; }
        string TruckMiles { get; set; }
        bool LocationsValid { get; set; }
    }
}
