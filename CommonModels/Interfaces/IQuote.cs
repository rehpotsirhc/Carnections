using Enums.Models;

namespace Common.Interfaces
{
    //ILocation pickup, ILocation delivery, ETrailerType trailerType, bool isOperable, IVehicleMinimal vehicleMinimal

    public interface IQuote :IHasId, IHasChangeDates
    {
        ILocation Pickup { get; }
        ILocation Delivery { get; }
        ETrailerType TrailerType { get; }
        bool VehicleIsOperable { get; }
        IVehicleMinimal Vehicle { get; }
    }
}
