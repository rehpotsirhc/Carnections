using Enums.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Interfaces
{
    public interface IVehicleTypeSize
    {
        EVehicleType Type { get; }
        IVehicleSizeWeight SizeWeight { get; }
    }
}
