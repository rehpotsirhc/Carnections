using Common.Interfaces;
using Enums.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class VehicleSizeWeight : IVehicleSizeWeight
    {
        public EVehicleSize Size { get; set; }
        public double Weight { get; set; }
    }
}
