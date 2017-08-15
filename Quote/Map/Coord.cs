using System;
using System.Collections.Generic;
using System.Text;

namespace Quote.Map
{
    public interface ICoord
    {
        int X { get; }
        int Y { get; }
    }

    public class Coord : ICoord, System.IEquatable<ICoord>
    {
        public int X { get; }
        public int Y { get; }

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
