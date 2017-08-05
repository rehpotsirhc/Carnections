using Common.Interfaces;
using Common.Models;
using GoogleDistance.Models;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleDistance
{
    //CityStateZip or location string => CityStateZip => Call Google Geocode => loop through results => CityStateZip each => find match based on matching city/state or zip strings =>
    //get lat lon from this result and return it



    public class LookupLonLat
    {

        public LookupLonLat()
        {

        }

        public static async Task<ILonLat> Lookup(string city, string state, string zip, StateNameForm stateNameForm)
        {
            ICityStateZipWithString standardizedLocation = CityStateZipBuilder.Build(city, state, zip, stateNameForm);
            var googleLonLat = await FindMatchTiered(standardizedLocation, stateNameForm);

            return ConvertToLonLat(googleLonLat);
        }
        public static async Task<ILonLat> Lookup(ICityStateZip location, StateNameForm stateNameForm)
        {
            return await Lookup(location.City, location.State, location.Zip, stateNameForm);
        }

        public static async Task<ILonLat> Lookup(string location, StateNameForm stateNameForm)
        {
            ICityStateZipWithString standardizedLocation = CityStateZipBuilder.Build(location, stateNameForm);
            var googleLonLat = await FindMatchTiered(standardizedLocation, stateNameForm);
            return ConvertToLonLat(googleLonLat);
        }

        //is this the best place for it?
        private static ILonLat ConvertToLonLat(GoogleLonLat googleLonLat)
        {
            return new LonLat()
            {
                Longitude = googleLonLat.Longitude,
                Latitude = googleLonLat.Latitude
            };
        }

        private static async Task<GoogleLonLat> FindMatchTiered(ICityStateZipWithString standardizedLocation, StateNameForm stateNameForm)
        {
            GoogleLonLat googleLatLon;
            googleLatLon = FindMatch(await GetGeocodeByFullLocation(standardizedLocation), standardizedLocation, stateNameForm);

            if (googleLatLon == null)
            {
                googleLatLon = FindMatch(await GetGeocodeByCityState(standardizedLocation.City, standardizedLocation.State, stateNameForm), standardizedLocation, stateNameForm);
            }
            if (googleLatLon == null)
            {
                googleLatLon = FindMatch(await GetGeocodeByZip(standardizedLocation.Zip), standardizedLocation, stateNameForm);
            }

            return googleLatLon;
        }
        private static GoogleLonLat FindMatch(GoogleGeocode geocode, ICityStateZipWithString standardizedLocation, StateNameForm stateNameForm)
        {
            if (geocode != null || geocode.Success)
            {
                foreach (Result r in geocode.Results)
                {
                    var geocodeLocation = CityStateZipBuilder.Build(r.FormattedAddress, stateNameForm);

                    if (standardizedLocation.Equals(geocodeLocation))
                        return r.Geometry.Location;
                }
            }
            return null;
        }

        private static async Task<GoogleGeocode> GetGeocodeByFullLocation(ICityStateZipWithString standardizedLocation)
        {
            return await CallGoogleGeocode.BuildEndpoint(standardizedLocation.FullAddress)?.GetGeoCode();
        }

        private static async Task<GoogleGeocode> GetGeocodeByCityState(string city, string state, StateNameForm stateNameForm)
        {
            ICityStateZipWithString standardizedLocation = CityStateZipBuilder.Build(city, state, null, stateNameForm);
            return await CallGoogleGeocode.BuildEndpoint(standardizedLocation.FullAddress)?.GetGeoCode();
        }

        private static async Task<GoogleGeocode> GetGeocodeByZip(string zip)
        {
            ICityStateZipWithString standardizedLocation = CityStateZipBuilder.Build(null, null, zip, StateNameForm.NoPreference);
            return await CallGoogleGeocode.BuildEndpoint(standardizedLocation.FullAddress)?.GetGeoCode();
        }

      

        





    }
}
