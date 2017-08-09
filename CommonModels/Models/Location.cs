using Common.Interfaces;
using Common.Models;

namespace Common.Models
{
    public class Location: CityStateZip, ILocation, ICityStateZip, ILonLat
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
