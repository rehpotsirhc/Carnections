using Common.Interfaces;
using Enums.Models;

namespace Common.Models
{
    public class VehicleTypeSize : IVehicleTypeSize
    {
        public EVehicleType Type { get; set; }
        public IVehicleSizeWeight SizeWeight { get; set; }
 
    }
}
