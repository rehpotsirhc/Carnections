using Common.Interfaces;

namespace VehicleData.Repositories
{
    public interface IVehicleRepository
    {
        IVehicleMinimal GetVehicle(string make, string model);
    }
}
