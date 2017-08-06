using Common.Interfaces;

namespace Common.Models
{
    public class LonLat :ILonLat
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
