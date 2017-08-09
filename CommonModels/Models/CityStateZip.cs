﻿using Common.Interfaces;
using GoogleDistance.Models;
using System;

namespace Common.Models
{
    public class CityStateZip : ICityStateZip
    {
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        public override bool Equals(object that)
        {
            return Equals((ICityStateZip)that);
        }

        public bool Equals(ICityStateZip that)
        {
            return CityStateZipBuilder.Equals(this, that);
        }
        public int DegreeOfEquals(ICityStateZip that)
        {
            return CityStateZipBuilder.DegreeOfEquals(this, that);
        }

        public override int GetHashCode()
        {
            return CityStateZipBuilder.GetHashCode(this);
        }

    }
}
