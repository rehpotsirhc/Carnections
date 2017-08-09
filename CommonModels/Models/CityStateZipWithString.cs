using Common.Interfaces;

namespace Common.Models
{
    class CityStateZipWithString : CityStateZip, ICityStateZipWithString
    {
        public string FullAddress { get; set; }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
