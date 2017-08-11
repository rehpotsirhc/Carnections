using Common.Interfaces;
using Common.Models;
using Common.Utils;
using Enums.Models;
using System;
using System.Collections.Generic;

namespace Quote
{
    public static class QuoteCalculator
    {
    
        public static IPrice CalculatePrice(this IEnumerable<ITransformedListing> listings, ETrailerType myTrailerType, bool myIsOperable, IVehicleMinimal vehicleMinimal)
        {
            if (listings == null)
                return PriceBuilder.Build(0, 0);

            double chargeForInop = 150;
            double sumPrice = 0;
            double count = 0;
            var vehicleFull = VehicleTransformer.Transform(vehicleMinimal);
            var traielrTypeWeight = new TrailerTypeWeight(myTrailerType);

            foreach (TransformedListing transformedListing in listings)
            {

                double price = transformedListing.Price / transformedListing.VehicleCount;

                price *= vehicleFull.TypeSize.SizeWeight.Weight / transformedListing.AverageVehicleWeight;
                price *= traielrTypeWeight.Weight / transformedListing.TrailerTypeWeight.Weight;

                if (!transformedListing.VehicleOperable && myIsOperable) // listing is inop, we're op so decrease price 
                    chargeForInop *= -1;
                else if (transformedListing.VehicleOperable == myIsOperable) //listing is same as my vehicle, so don't change
                    chargeForInop = 0;

                price += (chargeForInop * transformedListing.VehicleCount);

                if (price > 0)
                {
                    sumPrice += price;
                    count += 1;
                }
            }

            if (count == 0) return PriceBuilder.Build(0, 0);
            else
            {
                double price = (sumPrice / count).ToNearestQuarter();
                double deposit = price >= 1500 ? 200 : price < 150 ? 75 : 150;
                return PriceBuilder.Build(price, deposit);
            }

        }

        public static double ToNearestQuarter(this double number)
        {
            return Math.Round(number / 25.0) * 25;
        }
    }
}
