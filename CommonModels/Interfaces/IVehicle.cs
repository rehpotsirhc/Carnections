using Enums.Models;

namespace Common.Interfaces
{
    public interface IVehicleMinimal : IHasId
    {
        EVehicleType Type { get; }
        string Make { get; }
        string Model { get; }
    }
    public interface IVehicle : IVehicleMinimal
    {
        IVehicleTypeSize TypeSize { get; }
    }
}
