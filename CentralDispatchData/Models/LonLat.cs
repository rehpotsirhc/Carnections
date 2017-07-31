using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class LonLat :ILonLat
    {
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}
