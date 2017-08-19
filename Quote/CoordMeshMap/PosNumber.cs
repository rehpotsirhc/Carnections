using System;
using System.Collections.Generic;
using System.Text;

namespace Quote.CoordMeshMap
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
}
