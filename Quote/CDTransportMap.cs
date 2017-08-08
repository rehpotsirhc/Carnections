using CentralDispatchData.interfaces;
using Common.Interfaces;
using Quote;
using System.Collections.Generic;

namespace Quote
{
    public class CDTransportMap
    {


        //long range: -66.84(E) ==> -124.45(W)   *57.61
        //lat range:  25.13(S) ==> 49.35 (N)    *24.22

        //MESH long range: 64 ==> 126 *62 (extra for padding)
        //MESH lat range: 24 ==> 51   *27 (extra for padding)
        //MESH Unit length: .5, MESH is *124 x *54

        private CoordMeshWithItems<ICDListing> _locations;

        public CDTransportMap(IEnumerable<ICDListing> listings)
        {
            var settings = new CoordMeshSettings(.5, 64, 126, 24, 51);
            this._locations = new CoordMeshWithItems<ICDListing>(settings);

            foreach (ICDListing listing in listings)
            {
                this._locations.Add(listing.Pickup.Longitude, listing.Pickup.Latitude, listing);
            }
        }

        public IEnumerable<ICDListing> Search(ILonLat pickup, ILonLat delivery)
        {
            //Get all listings leaving from the pickup location
            var pickupListings = this._locations.GetItems(pickup.Longitude, pickup.Latitude);

            //get the coordinates of the delivery location
            var deliveryCoords = this._locations.FitLonLat(delivery.Longitude, delivery.Latitude);


            foreach (ICDListing pickupListing in pickupListings.Values)
            {
                //get the delivery coords of the listings leaving from the pickup location
                var coords = this._locations.FitLonLat(pickupListing.Delivery.Longitude, pickupListing.Delivery.Latitude);

                //return only those being delivered to the delivery location
                if (deliveryCoords.Equals(coords))
                    yield return pickupListing;
            }
        }

    }
}
