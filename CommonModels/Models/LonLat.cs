using Common.Interfaces;

namespace Common.Models
{
    public class LonLat :ILonLat
    {
        public int Id { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
