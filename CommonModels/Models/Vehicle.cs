using Common.Interfaces;
using Enums.Models;

namespace Common.Models
{
    public class VehicleMinimal : IVehicleMinimal
    {
        public int Id { get; set; }
        public EVehicleType Type { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
    }
    public class Vehicle : VehicleMinimal, IVehicle
    {
        public IVehicleTypeSize TypeSize { get; set; }
    }
}
