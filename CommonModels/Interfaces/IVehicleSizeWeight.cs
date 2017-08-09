using Enums.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Interfaces
{
    public interface IVehicleSizeWeight
    {
        EVehicleSize Size { get; }
        double Weight { get; }
    }
}
