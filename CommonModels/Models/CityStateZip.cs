using Common.Interfaces;

namespace Common.Models
{
    public class CityStateZip : ICityStateZip
    {
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }
}
