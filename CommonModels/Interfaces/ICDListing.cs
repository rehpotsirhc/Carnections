using CentralDispatchData.Models;
using Common.Models;

namespace CentralDispatchData.interfaces
{
    public interface ICDListing : IId
    {
        LonLat Pickup { get; set; }
        LonLat Delivery { get; set; }
        int ListingId { get; set; }
        double Price { get; set; }
        double PricePerMile { get; set; }
        bool VehicleOperable { get; set; }
        TrailerType ShipMethod { get; set; }
        string VehicleTypes { get; set; }
        string TruckMiles { get; set; }
        bool LocationsValid { get; set; }
    }
}
