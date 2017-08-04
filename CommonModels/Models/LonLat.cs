using Common.Interfaces;

namespace Common.Models
{
    public class LonLat :ILonLat
    {
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}
