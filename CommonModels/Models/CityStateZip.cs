using Common.Interfaces;
using GoogleDistance.Models;
using System;

namespace Common.Models
{
    public class CityStateZip : ICityStateZip
    {
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        public override bool Equals(object obj)
        {
            return CityStateZipBuilder.Equals(this, (CityStateZip)obj);
        }

        public override int GetHashCode()
        {
            return CityStateZipBuilder.GetHashCode(this);
        }

    }
}
