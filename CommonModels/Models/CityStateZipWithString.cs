using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    class CityStateZipWithString : CityStateZip, ICityStateZipWithString, ICityStateZip, ICityStateZipString
    {
        public string FullAddress { get; set; }
    }
}
