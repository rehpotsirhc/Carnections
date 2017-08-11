using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace VehicleData.Repositories
{
    public interface IVehicleRepository
    {
        IVehicleMinimal GetVehicle(string make, string model);
    }
}
