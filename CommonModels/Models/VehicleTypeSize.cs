using Common.Interfaces;
using Enums.Models;

namespace Common.Models
{
    public class VehicleTypeSize : IVehicleTypeSize, IHasId
    {
        public int Id { get; set; }
        public EVehicleType Type { get; set; }
        public VehicleSizeWeight SizeWeight { get; set; }
 
    }
}
