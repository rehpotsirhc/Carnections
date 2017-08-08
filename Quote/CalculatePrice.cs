using CentralDispatchData.interfaces;
using Common.Interfaces;
using Common.Models;
using Quote.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Quote
{
    public static class QuoteCalculator
    {
        public static async Task<Price> Calculate(this Task<ICDListings> listings, TrailerType myTrailerType, bool myIsOperable, VehicleType myVehicleType)
        {
            if (listings == null)
                return new Price(0, 0);

            double chargeForInop = 150;
            double sumPrice = 0;
            double count = 0;
            var vehicleTypeSize = new VehicleTypeSize(myVehicleType);
            var traielrTypeWeight = new TrailerTypeWeight(myTrailerType);

            foreach (ICDListing l in (await listings).Listings)
            {
                TransformedListing transformedListing = new TransformedListing(l);

                double price = transformedListing.ListingOriginal.Price / transformedListing.VehicleCount;

                price *= vehicleTypeSize.SizeWeight.Weight / transformedListing.AverageVehicleWeight;
                price *= traielrTypeWeight.Weight / transformedListing.TrailerTypeWeight.Weight;

                if (!transformedListing.ListingOriginal.VehicleOperable && myIsOperable) // listing is inop, we're op so decrease price 
                    chargeForInop *= -1;
                else if (transformedListing.ListingOriginal.VehicleOperable == myIsOperable) //listing is same as my vehicle, so don't change
                    chargeForInop = 0;

                price += (chargeForInop * transformedListing.VehicleCount);

                if (price > 0)
                {
                    sumPrice += price;
                    count += 1;
                }
            }

            if (count == 0) return new Price(0, 0);
            else
            {
                double price = (sumPrice / count).ToNearestQuarter();
                double deposit = price >= 1500 ? 200 : price < 150 ? 75 : 150;
                return new Price(price, deposit);
            }

        }

        public static double ToNearestQuarter(this double number)
        {
            return Math.Round(number / 25.0) * 25;
        }
    }
}
