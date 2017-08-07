using Common.Interfaces;
using Common.Models;
using Common.Utils;
using GoogleDistance.Models;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDistance
{
    internal static class CallGoogleGeocode
    {
        private const string API_KEY = "AIzaSyD0HHgCCE-aeckWjEJ7eIr-EMr8HYLVGVo";

   
        //somehwere in here we need to validate the GoogleDistanceMatrix object
        public static async Task<GoogleGeocode> GetGeoCode(this string endpoint)
        {
            if(string.IsNullOrWhiteSpace(endpoint))
                return null;

            string response = await HttpGet.Get(endpoint);
            if (response == null)
                return null;
            try
            {
                return JsonConvert.DeserializeObject<GoogleGeocode>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string BuildEndpoint(string location)
        {
            if (String.IsNullOrWhiteSpace(location))
                return null;

            var sb = new StringBuilder();
            sb.Append("https://maps.googleapis.com/maps/api/geocode/json?");
            sb.AppendFormat("{0}={1}", "address", location);
            return sb.ToString();
        }

    }
}
