
namespace Common.Interfaces
{
    public interface ICityStateZip
    {
        string City { get; }
        string State { get; }
        string Zip { get; }

        int DegreeOfEquals(ICityStateZip that);
    }
}
