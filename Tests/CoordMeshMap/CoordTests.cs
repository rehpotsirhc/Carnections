using Quote.CoordMeshMap;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CoordMeshMap.Tests
{
    public class CoordTests
    {
        [Fact]
        public void Coords__IsNegative_ThrowsArgumentOutOfRangeException()
        {
            try
            {
                ICoord coord = new Coord(-111, 0);
            }

            catch (ArgumentOutOfRangeException)
            {
                Assert.True(true);
            }
        }

        [Fact]
        public void Coords_AreEqual()
        {
            ICoord coord1 = new Coord(1, 1);
            ICoord coord2 = new Coord(1, 1);
            Assert.True(coord1.Equals(coord2));
            Assert.True(coord2.Equals(coord1));


            coord1 = new Coord(777, 777);
            coord2 = new Coord(777, 777);
            Assert.True(coord1.Equals(coord2));
            Assert.True(coord2.Equals(coord1));
        }

        [Fact]
        public void Coords_AreNotEqual()
        {
            ICoord coord1 = new Coord(1, 2);
            ICoord coord2 = new Coord(1, 1);
            Assert.False(coord1.Equals(coord2));
            Assert.False(coord2.Equals(coord1));

            coord1 = new Coord(2, 1);
            coord2 = new Coord(1, 1);
            Assert.False(coord1.Equals(coord2));
            Assert.False(coord2.Equals(coord1));

            coord1 = new Coord(1, 1);
            coord2 = new Coord(1, 2);
            Assert.False(coord1.Equals(coord2));
            Assert.False(coord2.Equals(coord1));


            coord1 = new Coord(1, 1);
            coord2 = new Coord(2, 1);
            Assert.False(coord1.Equals(coord2));
            Assert.False(coord2.Equals(coord1));


            coord1 = new Coord(777, 1000);
            coord2 = new Coord(666, 777);
            Assert.False(coord1.Equals(coord2));
            Assert.False(coord2.Equals(coord1));
        }
    }
}
