using System;
using System.Collections.Generic;
using System.Text;

namespace Quote.Map
{
    public class PosNumber
    {
        public double AsDouble { get; }
        public PosNumber(double theNumber)
        {
            if (theNumber < 0)
                throw new ArgumentOutOfRangeException("theNumber", theNumber, "Can't create a positive number object with a negative number");

            this.AsDouble = theNumber;
        }
        public static implicit operator double(PosNumber theNumber)
        {
            return theNumber.AsDouble;
        }
        public static implicit operator PosNumber(double theNumber)
        {
            return new PosNumber(theNumber);
        }
        public static bool operator ==(PosNumber left, PosNumber right)
        {
            return left.AsDouble == right.AsDouble;
        }
        public static bool operator !=(PosNumber left, PosNumber right)
        {
            return left.AsDouble != right.AsDouble;
        }
        public static bool operator >(PosNumber left, PosNumber right)
        {
            return left.AsDouble > right.AsDouble;
        }
        public static bool operator <(PosNumber left, PosNumber right)
        {
            return left.AsDouble < right.AsDouble;
        }
        public static bool operator >=(PosNumber left, PosNumber right)
        {
            return left.AsDouble >= right.AsDouble;
        }
        public static bool operator <=(PosNumber left, PosNumber right)
        {
            return left.AsDouble <= right.AsDouble;
        }

        public override bool Equals(object obj)
        {
            return this.AsDouble == ((PosNumber)obj).AsDouble;
        }
        public override int GetHashCode()
        {
            unchecked //to avoid overflowing int
            {
                return 17 * 23 + this.AsDouble.GetHashCode();
            }
        }
    }
    public class PosInt : PosNumber
    {
        public int AsInt { get { return (int)AsDouble; } }
        public PosInt(int theNumber) : base(theNumber) { }

        public static implicit operator int(PosInt theNumber)
        {
            return theNumber.AsInt;
        }
        public static implicit operator PosInt(int theNumber)
        {
            return new PosInt(theNumber);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class CoordMeshSettings
    {
        public readonly PosNumber RESOLUTION;
        public readonly PosInt LON_MIN;
        public readonly PosInt LON_MAX;
        public readonly PosInt LAT_MIN;
        public readonly PosInt LAT_MAX;

        public CoordMeshSettings(double resolution, int lonMin, int lonMax, int latMin, int latMax)
        {
            this.RESOLUTION = resolution;
            this.LON_MIN = lonMin;
            this.LON_MAX = lonMax;
            this.LAT_MIN = latMin;
            this.LAT_MAX = latMax;

            if (!SettingsAreValid())
                throw new ArgumentException("The coord mesh settings are invalid. The max's have to be >= to the min's");
        }
        public bool SettingsAreValid()
        {
            return this.LON_MAX >= this.LON_MIN &&
                this.LAT_MAX >= this.LAT_MIN;
        }
    }
}
