using Enums.Models;

namespace Common.Interfaces
{
    public interface IVehicleTypeSize
    {
        EVehicleType Type { get; }
        IVehicleSizeWeight SizeWeight { get; }
    }
}
