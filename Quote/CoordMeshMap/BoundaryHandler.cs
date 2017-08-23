using System;
using System.Collections.Generic;
using System.Text;

namespace Quote.CoordMeshMap
{
 

    public class BoundaryHandler
    {
        public PosInt Coord { get;}
        public bool Ok { get; }

        /// <summary>
        /// Enforces the coordinate to be >= 0 and the length to be > 0. If either are not, "OK" is set to false
        /// </summary>
        /// <param name="coord">The coordinate</param>
        /// <param name="length">The length</param>
        public BoundaryHandler(int coord)
        {
            try
            {
                Coord = new PosInt(coord);
                this.Ok = true;
            }
            catch (ArgumentOutOfRangeException e)
            {
                //TODO: log?
                Coord = null;
                this.Ok = false;
            }
        }

        public delegate BoundaryHandler BoundaryHandleFunc(int coord, PosInt length);

        public static BoundaryHandler HandleBoundary_ConstrainOutOfBounds(int coord, PosInt length)
        {
            if (coord >= length.AsInt)
                coord = length - 1;
            else if (coord < 0)
                coord = 0;

            return new BoundaryHandler(coord);
        }

        public static BoundaryHandler HandleBoundary_IgnoreOutOfBounds(int coord, PosInt length)
        {
            if (coord >= length.AsInt)
                coord = -1;
            return new BoundaryHandler(coord);
        }
    }
}
