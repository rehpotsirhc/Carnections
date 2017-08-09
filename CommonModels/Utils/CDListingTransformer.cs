using Common.Interfaces;
using Common.Models;
using Enums.Models;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Common.Utils
{
    public class CDListingTransformer
    {
        public static ITransformedListing Transform(ICDListing originalListing)
        {
            IList<IVehicleTypeSize> vehicleTypesSizes;
            double averageVehicleWeight;
            int vehicleCount;
            ParseVehicles(originalListing, out vehicleTypesSizes, out averageVehicleWeight, out vehicleCount);
            return new TransformedListing()
            {
                Delivery = originalListing.Delivery,
                ListingId = originalListing.ListingId,
                LocationsValid = originalListing.LocationsValid,
                Pickup = originalListing.Pickup,
                Price = originalListing.Price,
                PricePerMile = originalListing.PricePerMile,
                ShipMethod = originalListing.ShipMethod,
                TruckMiles = originalListing.TruckMiles,
                VehicleOperable = originalListing.VehicleOperable,
                VehicleTypes = originalListing.VehicleTypes,
                TrailerTypeWeight = new TrailerTypeWeight(originalListing.ShipMethod),
                MilesInterpolated = originalListing.PricePerMile > 0 ? (int)(originalListing.Price / originalListing.PricePerMile) : 0,
                VehicleTypesSizes = vehicleTypesSizes,
                AverageVehicleWeight = averageVehicleWeight,
                VehicleCount = vehicleCount
            };
        }

        private static void ParseVehicles(ICDListing originalListing, out IList<IVehicleTypeSize> vehicleTypeSizes, out double averageVehicleWeight, out int vehicleCount)
        {
            vehicleTypeSizes = new List<IVehicleTypeSize>();
            averageVehicleWeight = 0;
            vehicleCount = 0;
            string[] vehicleStringWithQtyParts = originalListing.VehicleTypes.Replace("(", "").Replace(")", "").Trim().Split(',');
            foreach (string vehicleStringWithQty in vehicleStringWithQtyParts)
            {
                int qty;
                string vehicleString;
                MatchCollection matches = Regex.Matches(vehicleStringWithQty.Trim(), @"^\d");
                if (matches.Count > 0)
                {
                    string numberTemp = matches[0].Groups[0].Value;
                    qty = int.Parse(numberTemp.Trim());
                    vehicleString = vehicleStringWithQty.Replace(numberTemp, "").Trim();
                }
                else
                {
                    qty = 1;
                    vehicleString = vehicleStringWithQty.Trim();
                }
                for (int i = 0; i < qty; i++)
                {
                    EVehicleType vehicleType;
                    if (Enum.TryParse(Regex.Replace(vehicleString, "\\s+", ""), out vehicleType))
                    {
                        var typeSize = new VehicleTypeSize(vehicleType);
                        vehicleTypeSizes.Add(typeSize);
                        averageVehicleWeight += typeSize.SizeWeight.Weight;
                        vehicleCount += 1;
                    }
                }
            }
            averageVehicleWeight /= vehicleCount;
        }
    }
}

