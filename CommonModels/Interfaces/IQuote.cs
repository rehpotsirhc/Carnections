using Enums.Models;
using System;
using System.Collections.Generic;
using System.Text;

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
