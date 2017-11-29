using Common.Interfaces;
using Enums.Models;

namespace Common.Models
{
    public class VehicleSizeWeight : IVehicleSizeWeight, IHasId
    {
        public int Id { get; set; }
        public EVehicleSize Size { get; set; }
        public double Weight { get; set; }
    }
}
