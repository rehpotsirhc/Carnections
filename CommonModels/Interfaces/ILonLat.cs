using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Interfaces
{
    public interface ILonLat :IHasId
    {
        double Longitude { get; }
        double Latitude { get; }
    }
}
