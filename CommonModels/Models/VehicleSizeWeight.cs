using Common.Interfaces;
using Enums.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class VehicleSizeWeight : IVehicleSizeWeight
    {
        public EVehicleSize Size { get; private set; }
        public double Weight { get; private set; }
        public VehicleSizeWeight(EVehicleSize size)
        {
            this.Size = size;
            if (this.Size == EVehicleSize.Small)
                this.Weight = 1;
            else if (this.Size == EVehicleSize.Large)
                this.Weight = 1.25;
        }
    }
}
