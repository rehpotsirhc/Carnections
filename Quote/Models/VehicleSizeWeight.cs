using System;
using System.Collections.Generic;
using System.Text;

namespace Quote.Models
{
    public class VehicleSizeWeight
    {
        public VehicleSize Size { get; private set; }
        public double Weight { get; private set; }
        public VehicleSizeWeight(VehicleSize size)
        {
            this.Size = size;
            if (this.Size == VehicleSize.Small)
                this.Weight = 1;
            else if (this.Size == VehicleSize.Large)
                this.Weight = 1.25;
        }
    }
}
