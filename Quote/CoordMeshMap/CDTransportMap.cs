using CentralDispatchData.Repositories;
using Common.Interfaces;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using System;

namespace Quote.CoordMeshMap
{
    public class CDTransportMap
    {
        private const string CACHE_KEY = "CDTransportMap_KEY";
        private const int EXPIRATION_IN_DAYS = 3; //this is also how often we should scrape central dispatch

        //long range: -66.84(E) ==> -124.45(W)   *57.61
        //lat range:  25.13(S) ==> 49.35 (N)    *24.22

        //MESH long range: 64 ==> 126 *62 (extra for padding)
        //MESH lat range: 24 ==> 51   *27 (extra for padding)
        //MESH Unit length: .5, MESH is *124 x *54

        private CoordMeshWithItems<ITransformedListing> _locations;

        public CDTransportMap(IEnumerable<ITransformedListing> listings)
        {
            var settings = new CoordMeshSettings(.5, 64, 126, 24, 51);
            this._locations = new CoordMeshWithItems<ITransformedListing>(settings);

            foreach (ITransformedListing listing in listings)
            {
                this._locations.Add(listing.Pickup.Longitude, listing.Pickup.Latitude, listing);
            }
        }

        public IEnumerable<ITransformedListing> Search(ILonLat pickup, ILonLat delivery)
        {
            //Get all listings leaving from the pickup location
            var pickupListings = this._locations.GetItems(pickup.Longitude, pickup.Latitude);

            //get the coordinates of the delivery location
            var deliveryCoords = this._locations.FitLonLat(delivery.Longitude, delivery.Latitude);


            foreach (ITransformedListing pickupListing in pickupListings.Values)
            {
                //get the delivery coords of the listings leaving from the pickup location
                var coords = this._locations.FitLonLat(pickupListing.Delivery.Longitude, pickupListing.Delivery.Latitude);

                //return only those being delivered to the delivery location
                if (deliveryCoords.Equals(coords))
                    yield return pickupListing;
            }
        }

        public static async Task<CDTransportMap> GetMap(IMemoryCache cache, ICDListingRepository repository)
        {
            return await cache.GetOrCreateAsync<CDTransportMap>(CACHE_KEY, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromDays(EXPIRATION_IN_DAYS);
                return Task.FromResult(new CDTransportMap(repository.GetAll()));
            });
        }
    }
}
