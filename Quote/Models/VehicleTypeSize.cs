using System;
using System.Collections.Generic;
using System.Text;

namespace Quote.Models
{
    public class VehicleTypeSize
    {
        public VehicleType Type { get; private set; }
        public VehicleSizeWeight SizeWeight { get; private set; }
        public VehicleTypeSize(VehicleType type)
        {
            this.Type = type;
            if (this.Type == VehicleType.Car || this.Type == VehicleType.Motorcycle || this.Type == VehicleType.ATV)
                SizeWeight = new VehicleSizeWeight(VehicleSize.Small);
            else
                SizeWeight = new VehicleSizeWeight(VehicleSize.Large);
        }
    }
}
