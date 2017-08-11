using Common.Interfaces;
using Enums.Models;

namespace Common.Models
{
    public class VehicleSizeWeight : IVehicleSizeWeight
    {
        public EVehicleSize Size { get; set; }
        public double Weight { get; set; }
    }
}
