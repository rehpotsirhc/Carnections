using Common.Interfaces;
using Common.Models;

namespace ScrapeCentralDispatch.Models
{
    public class Location: CityStateZip, ILocation, ICityStateZip, ILonLat
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
