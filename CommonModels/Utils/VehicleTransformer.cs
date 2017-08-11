using Common.Interfaces;
using Common.Models;
using Enums.Models;

namespace Common.Utils
{
    public static class VehicleTransformer
    {
        public static IVehicle Transform(IVehicleMinimal baseVehicle)
        {
            return new Vehicle()
            {
                Id = baseVehicle.Id,
                Make = baseVehicle.Make,
                Model = baseVehicle.Model,
                Type = baseVehicle.Type,
                TypeSize = Build(baseVehicle.Type)
            };
        }

        public static IVehicleSizeWeight Build(EVehicleSize size)
        {
            double weight = 1;
            if (size == EVehicleSize.Small)
                weight = 1;
            else if (size == EVehicleSize.Large)
                weight = 1.25;

            return new VehicleSizeWeight()
            {
                Size = size,
                Weight = weight
            };
        }

        public static IVehicleTypeSize Build(EVehicleType type)
        {
            IVehicleSizeWeight sizeWeight;
            if (type== EVehicleType.Car || type == EVehicleType.Motorcycle || type == EVehicleType.ATV)
                sizeWeight = Build(EVehicleSize.Small);
            else
                sizeWeight = Build(EVehicleSize.Large);

            return new VehicleTypeSize()
            {
                Type = type,
                SizeWeight = sizeWeight
            };
        }
    }
}
