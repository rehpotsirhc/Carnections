using CentralDispatchData.interfaces;
using Common.Interfaces;
using Quote.Models;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Quote
{

    //assumes the listings it receives are valid
    public class TransformedListing : IHasId
    {
        public int Id { get; private set; }
        public ICDListing ListingOriginal { get; private set; }
        public TransformedListing(ICDListing listingOriginal)
        {
            this.ListingOriginal = listingOriginal;
            this.Id = ListingOriginal.Id;
            ParseVehicles();
            ParseTrailerType();
            double pricePerMile = ListingOriginal.PricePerMile;
            this.MilesInterpolated = pricePerMile > 0 ? (int)(ListingOriginal.Price / ListingOriginal.PricePerMile) : 0;
        }
        private void ParseVehicles()
        {
            string[] vehicleStringWithQtyParts = ListingOriginal.VehicleTypes.Replace("(", "").Replace(")", "").Trim().Split(',');
            VehicleTypesSizes = new List<VehicleTypeSize>();
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
                    VehicleType vehicleType;
                    if (Enum.TryParse(Regex.Replace(vehicleString, "\\s+", ""), out vehicleType))
                    {
                        var typeSize = new VehicleTypeSize(vehicleType);
                        VehicleTypesSizes.Add(typeSize);
                        AverageVehicleWeight += typeSize.SizeWeight.Weight;
                        VehicleCount += 1;
                    }
                }
            }
            AverageVehicleWeight /= VehicleCount;
        }

        private void ParseTrailerType()
        {
            TrailerTypeWeight = new TrailerTypeWeight(ListingOriginal.ShipMethod);
        }

        public TrailerTypeWeight TrailerTypeWeight { get; private set; }

        public IList<VehicleTypeSize> VehicleTypesSizes { get; private set; }
        public double AverageVehicleWeight { get; private set; }
        public int VehicleCount { get; private set; }
        public int MilesInterpolated { get; private set; }
    }
}
