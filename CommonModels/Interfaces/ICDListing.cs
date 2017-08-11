using Enums.Models;

namespace Common.Interfaces
{
    public interface ICDListing : IHasListingId
    {
        ILonLat Pickup { get; }
        ILonLat Delivery { get; }
        double Price { get; }
        double PricePerMile { get; }
        bool VehicleOperable { get; }
        ETrailerType ShipMethod { get; }
        string VehicleTypes { get; }
        string TruckMiles { get; }
        bool LocationsValid { get; }
    }
}
