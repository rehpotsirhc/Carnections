using Common.Models;
using System.Collections.Generic;

namespace Common.Interfaces
{
    public interface ITransformedListing : ICDListing, IHasId, IHasListingIdAndChangeDates
    {
        TrailerTypeWeight TrailerTypeWeight { get; }
        List<VehicleTypeSize> VehicleTypesSizes { get; }
        double AverageVehicleWeight { get; }
        int VehicleCount { get; }
        int MilesInterpolated { get; }
    }
}
