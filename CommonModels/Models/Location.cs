using Common.Interfaces;
using Common.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScrapeCentralDispatch.Models
{
    public class Location: CityStateZip, ILocation, ICityStateZip, ILonLat
    {
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}
