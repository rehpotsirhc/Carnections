using Quote.CoordMeshMap;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CoordMeshMap.Tests
{
    //the exact number is these tests don't really matter, as long as the relations between them remain true
    public class PosIntTests
    {

        [Fact]
        public void IsNegative_ThrowsArgumentOutOfRangeException()
        {
            try
            {
                var PosInt = new PosInt(-68);
            }
            catch (ArgumentOutOfRangeException)
            {
                Assert.True(true);
            }
        }

        [Fact]
        public void IsZero_ZeroNotAllowed_ThrowsArgumentOutOfRangeException()
        {
            try
            {
                var PosInt = new PosInt(0, false);
            }
            catch (ArgumentOutOfRangeException)
            {
                Assert.True(true);
            }
        }

        [Fact]
        public void Implicit_ToDouble()
        {
            int d0 = new PosInt(0);
            Assert.Equal(0, d0);

            int d1 = new PosInt(1);
            Assert.Equal(1, d1);

            int d2 = new PosInt(int.MaxValue);
            Assert.Equal(int.MaxValue, d2);
        }

        [Fact]
        public void Implicit_FromDouble()
        {
            PosInt d0 = 0;
            Assert.Equal(0, d0.AsDouble);

            int d1 = new PosInt(56);
            Assert.Equal(56, d1);

            int d2 = new PosInt(999);
            Assert.Equal(999, d2);

        }

        [Fact]
        public void Explicit_Equals()
        {
            PosInt p1 = new PosInt(66);
            PosInt p2 = new PosInt(66);
            Assert.True(p1 == p2);

            p1 = new PosInt(0);
            p2 = new PosInt(0);
            Assert.True(p1 == p2);

        }

        [Fact]
        public void Explicit_NotEquals()
        {
            PosInt p1 = new PosInt(661);
            PosInt p2 = new PosInt(67);
            Assert.True(p1 != p2);

            p1 = new PosInt(0);
            p2 = new PosInt(100);
            Assert.True(p1 != p2);
        }

        [Fact]
        public void Explicit_GreaterThan()
        {
            PosInt p1 = new PosInt(50431);
            PosInt p2 = new PosInt(400);
            Assert.True(p1 > p2);

            p1 = new PosInt(100);
            p2 = new PosInt(0);
            Assert.True(p1 > p2);
        }

        [Fact]
        public void Explicit_GreaterThanOrEqual()
        {
            PosInt p1 = new PosInt(66);
            PosInt p2 = new PosInt(66);
            Assert.True(p1 >= p2);

            p1 = new PosInt(99);
            p2 = new PosInt(90);
            Assert.True(p1 >= p2);
        }

        [Fact]
        public void Explicit_LessThan()
        {
            PosInt p1 = new PosInt(0);
            PosInt p2 = new PosInt(4001);
            Assert.True(p1 < p2);

            p1 = new PosInt(int.MaxValue - int.MaxValue / 99999999);
            p2 = new PosInt(int.MaxValue);
            Assert.True(p1 < p2);
        }


        [Fact]
        public void Explicit_LessThanOrEqual()
        {
            PosInt p1 = new PosInt(0);
            PosInt p2 = new PosInt(0);
            Assert.True(p1 <= p2);

            p1 = new PosInt(int.MaxValue);
            p2 = new PosInt(int.MaxValue);
            Assert.True(p1 <= p2);
        }

        [Fact]
        public void Override_Equals()
        {
            PosInt p1 = new PosInt(0);
            PosInt p2 = new PosInt(0);
            Assert.True(p1.Equals(p2));

            p1 = new PosInt(int.MaxValue);
            p2 = new PosInt(int.MaxValue);
            Assert.True(p1.Equals(p2));


            p1 = new PosInt(99);
            p2 = new PosInt(98);
            Assert.False(p1.Equals(p2));

        }

        [Fact]
        public void Override_HashCode()
        {
            PosInt p1 = new PosInt(0);
            PosInt p2 = new PosInt(0);
            Assert.Equal(p1.GetHashCode(), p2.GetHashCode());

            p1 = new PosInt(0);
            p2 = new PosInt(1);
            Assert.NotEqual(p1.GetHashCode(), p2.GetHashCode());
        }
    }
}

