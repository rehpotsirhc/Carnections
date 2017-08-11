using Enums.Models;

namespace Common.Interfaces
{
    public interface IVehicleSizeWeight
    {
        EVehicleSize Size { get; }
        double Weight { get; }
    }
}
