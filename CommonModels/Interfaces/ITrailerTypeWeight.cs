using Enums.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Interfaces
{
    public interface ITrailerTypeWeight
    {
        ETrailerType Type { get; }
        double Weight { get; }
    }
}
