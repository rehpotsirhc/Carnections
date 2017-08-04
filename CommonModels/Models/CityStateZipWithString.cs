using Common.Interfaces;

namespace Common.Models
{
    class CityStateZipWithString : CityStateZip, ICityStateZipWithString, ICityStateZip, ICityStateZipString
    {
        public string FullAddress { get; set; }
    }
}
