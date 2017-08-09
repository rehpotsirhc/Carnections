﻿using Common.Interfaces;
using Enums.Models;

namespace Common.Models
{
    public class TrailerTypeWeight : ITrailerTypeWeight
    {
        public ETrailerType Type { get; private set; }
        public double Weight { get; private set; }
        public TrailerTypeWeight(ETrailerType type)
        {
            this.Type = type;
            if (this.Type == ETrailerType.Open || this.Type == ETrailerType.Driveaway)
                Weight = 1;
            else
                Weight = 1.5;
        }
    }
}
