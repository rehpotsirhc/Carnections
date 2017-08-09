using System;
using System.Collections.Generic;

namespace Common.Interfaces
{
    public interface ITransformedListing : ICDListing, IHasId, IHasListingIdAndChangeDates
    {
        ITrailerTypeWeight TrailerTypeWeight { get; }
        IList<IVehicleTypeSize> VehicleTypesSizes { get; }
        double AverageVehicleWeight { get; }
        int VehicleCount { get; }
        int MilesInterpolated { get; }
    }
}
