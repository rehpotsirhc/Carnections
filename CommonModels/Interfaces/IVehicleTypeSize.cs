using Common.Models;
using Enums.Models;

namespace Common.Interfaces
{
    public interface IVehicleTypeSize
    {
        EVehicleType Type { get; }
        VehicleSizeWeight SizeWeight { get; }
    }
}
