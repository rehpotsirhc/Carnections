using Common.Interfaces;
using Common.Models;

namespace ScrapeCentralDispatch.Models
{
    public class Location: CityStateZip, ILocation, ICityStateZip, ILonLat
    {
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}
