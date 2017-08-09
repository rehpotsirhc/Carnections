using Common.Interfaces;
using Common.Models;
using Enums.Models;

namespace Common.Models
{
    public class VehicleTypeSize : IVehicleTypeSize
    {
        public EVehicleType Type { get; private set; }
        public IVehicleSizeWeight SizeWeight { get; private set; }
        public VehicleTypeSize(EVehicleType type)
        {
            this.Type = type;
            if (this.Type == EVehicleType.Car || this.Type == EVehicleType.Motorcycle || this.Type == EVehicleType.ATV)
                SizeWeight = new VehicleSizeWeight(EVehicleSize.Small);
            else
                SizeWeight = new VehicleSizeWeight(EVehicleSize.Large);
        }
    }
}
