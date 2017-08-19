using System;
using System.Collections.Generic;
using System.Text;

namespace Quote.CoordMeshMap
{
    public interface ICoord : System.IEquatable<ICoord>
    {
        PosInt X { get; }
        PosInt Y { get; }
    }

    public class Coord : ICoord
    {
        public PosInt X { get; }
        public PosInt Y { get; }

        public Coord(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public bool Equals(ICoord other)
        {
            return other != null && other.X == this.X && other.Y == this.Y;
        }
    }
}
