using Enums.Models;

namespace Common.Interfaces
{
    public interface ITrailerTypeWeight
    {
        ETrailerType Type { get; }
        double Weight { get; }
    }
}
