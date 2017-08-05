using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleDistance.Models
{

  
    internal class GoogleGeocode
    {
        public List<Result> Results { get; set; }
        public string Status { get; set; }

        public bool Success
        {
            get
            {
                return Status == "OK";
            }
        }
    }

    internal class Result
    {
        [JsonProperty("formatted_address")]
        public string FormattedAddress { get; set; }
        public Geometry Geometry { get; set; }
 
    }

    internal class Bounds
    {
        public GoogleLonLat Northeast { get; set; }
        public GoogleLonLat Southwest { get; set; }
    }

    internal class GoogleLonLat
    {
        [JsonProperty("lng")]
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }

    internal class Geometry
    {
        public Bounds Bounds { get; set; }
        public GoogleLonLat Location { get; set; }

    }

   
}
