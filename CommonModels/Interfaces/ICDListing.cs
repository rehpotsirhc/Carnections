using Enums.Models;

namespace Common.Interfaces
{
    public interface ICDListing : IHasListingId
    {
        ILonLat Pickup { get; set; }
        ILonLat Delivery { get; set; }
        double Price { get; set; }
        double PricePerMile { get; set; }
        bool VehicleOperable { get; set; }
        ETrailerType ShipMethod { get; set; }
        string VehicleTypes { get; set; }
        string TruckMiles { get; set; }
        bool LocationsValid { get; set; }
    }
}
