using Common.Enums;
using Common.Interfaces;
using Common.Models;
using GoogleDistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleDistance
{
    //CityStateZip or location string => CityStateZip => Call Google Geocode => loop through results => CityStateZip each => find match based on matching city/state or zip strings =>
    //get lat lon from this result and return it



    public class LookupLonLat
    {
        private static readonly int DEGREE_OF_EQUALS_MIN = 1;
        private static readonly int DEGREE_OF_EQUALS_STOP_SEARCH = 3;
        public LookupLonLat()
        {

        }

        public static async Task<ILocation> Lookup(string city, string state, string zip, EStateNameForm stateNameForm)
        {
            ICityStateZipWithString standardizedLocation = CityStateZipBuilder.Build(city, state, zip, stateNameForm);
            return await FindMatchTiered(standardizedLocation, stateNameForm);

        }
        public static async Task<ILocation> Lookup(ICityStateZip location, EStateNameForm stateNameForm)
        {
            return await Lookup(location.City, location.State, location.Zip, stateNameForm);
        }

        public static async Task<ILocation> Lookup(string location, EStateNameForm stateNameForm)
        {
            ICityStateZipWithString standardizedLocation = CityStateZipBuilder.Build(location, stateNameForm);
            return await FindMatchTiered(standardizedLocation, stateNameForm);
        }

        private static async Task<ILocation> FindMatchTiered(ICityStateZipWithString standardizedLocation, EStateNameForm stateNameForm)
        {
            var googleLatLons = new List<Tuple<int, ILocation>>();
            Tuple<int, ILocation> best = null;
            bool continueSearch = FindMatch(await GetGeocodeByFullLocation(standardizedLocation), standardizedLocation, stateNameForm, googleLatLons, ref best);

            if (continueSearch && !String.IsNullOrWhiteSpace(standardizedLocation.City) && !String.IsNullOrWhiteSpace(standardizedLocation.State))
            {
                continueSearch = FindMatch(await GetGeocodeByCityState(standardizedLocation.City, standardizedLocation.State, stateNameForm), standardizedLocation, stateNameForm, googleLatLons, ref best);
            }
            if (continueSearch && !String.IsNullOrWhiteSpace(standardizedLocation.Zip))
            {
                continueSearch = FindMatch(await GetGeocodeByZip(standardizedLocation.Zip), standardizedLocation, stateNameForm, googleLatLons, ref best);
            }

            return best?.Item2;
        }
        private static bool FindMatch(GoogleGeocode geocode, ICityStateZipWithString standardizedLocation, EStateNameForm stateNameForm, List<Tuple<int, ILocation>> googleLatLons, ref Tuple<int, ILocation> best)
        {
            if (googleLatLons == null)
                googleLatLons = new List<Tuple<int, ILocation>>();

            if (geocode != null || geocode.Success)
            {
                foreach (Result r in geocode.Results)
                {
                    var geocodeLocation = CityStateZipBuilder.Build(r.FormattedAddress, stateNameForm);
                    int degreeOfEquals = standardizedLocation.DegreeOfEquals(geocodeLocation);

                    if (degreeOfEquals >= DEGREE_OF_EQUALS_MIN)
                    {
                        var latLon = r.Geometry.Location;

                        ILocation location = new Location()
                        {
                            City = geocodeLocation.City,
                            State = geocodeLocation.State,
                            Zip = geocodeLocation.Zip,
                            Latitude = latLon.Latitude,
                            Longitude = latLon.Longitude
                        };

                        var latLonTuple = new Tuple<int, ILocation>(degreeOfEquals, location);
                        if (best == null || googleLatLons.Where(t => t.Item1 > degreeOfEquals).Count() == 0)
                            best = latLonTuple;

                        googleLatLons.Add(latLonTuple);

                        if (degreeOfEquals >= DEGREE_OF_EQUALS_STOP_SEARCH)
                            return false;
                    }
                }
            }
            return true;
        }

        private static async Task<GoogleGeocode> GetGeocodeByFullLocation(ICityStateZipWithString standardizedLocation)
        {
            return await CallGoogleGeocode.BuildEndpoint(standardizedLocation.FullAddress)?.GetGeoCode();
        }

        private static async Task<GoogleGeocode> GetGeocodeByCityState(string city, string state, EStateNameForm stateNameForm)
        {
            ICityStateZipWithString standardizedLocation = CityStateZipBuilder.Build(city, state, null, stateNameForm);
            return await CallGoogleGeocode.BuildEndpoint(standardizedLocation.FullAddress)?.GetGeoCode();
        }

        private static async Task<GoogleGeocode> GetGeocodeByZip(string zip)
        {
            ICityStateZipWithString standardizedLocation = CityStateZipBuilder.Build(null, null, zip, EStateNameForm.NoPreference);
            return await CallGoogleGeocode.BuildEndpoint(standardizedLocation.FullAddress)?.GetGeoCode();
        }









    }
}
