using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class CityStateZip : ICityStateZip
    {
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }
}
