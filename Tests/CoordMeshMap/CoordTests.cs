using Quote.CoordMeshMap;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Tests.CoordMeshMap
{
    public class CoordTests
    {
        [Fact]
        public void Coords_AreEqual()
        {
            ICoord coord = new Coord(-1, -1);
        }
    }
}
