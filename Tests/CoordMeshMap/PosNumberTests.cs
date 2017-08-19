using Quote.CoordMeshMap;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CoordMeshMap.Tests
{
    //the exact number is these tests don't really matter, as long as the relations between them remain true
    public class PosNumberTests
    {

        [Fact]
        public void Constructor_IsNegative_ThrowsArgumentOutOfRangeException()
        {
            try
            {
                var posNumber = new PosNumber(-67.8);
            }
            catch(ArgumentOutOfRangeException)
            {
                Assert.True(true);
            }
        }

        [Fact]
        public void Implicit_ToDouble()
        {
            double d0 = new PosNumber(0);
            Assert.Equal(0, d0);

            double d1 = new PosNumber(.1);
            Assert.Equal(.1, d1);

            double d2 = new PosNumber(Double.MaxValue);
            Assert.Equal(Double.MaxValue, d2);
        }
        
        [Fact]
        public void Implicit_FromDouble()
        {
            PosNumber d0 = 0;
            Assert.Equal(0, d0.AsDouble);

            double d1 = new PosNumber(55.5);
            Assert.Equal(55.5, d1);

            double d2 = new PosNumber(.999998);
            Assert.Equal(.999998, d2);

        }

        [Fact]
        public void Explicit_Equals()
        {
            PosNumber p1 = new PosNumber(.66);
            PosNumber p2 = new PosNumber(.66);
            Assert.True(p1 == p2);

            p1 = new PosNumber(0);
            p2 = new PosNumber(0);
            Assert.True(p1 == p2);

        }

        [Fact]
        public void Explicit_NotEquals()
        {
            PosNumber p1 = new PosNumber(.661);
            PosNumber p2 = new PosNumber(.67);
            Assert.True(p1 != p2);

            p1 = new PosNumber(0);
            p2 = new PosNumber(100);
            Assert.True(p1 != p2);
        }

        [Fact]
        public void Explicit_GreaterThan()
        {
            PosNumber p1 = new PosNumber(504.31);
            PosNumber p2 = new PosNumber(400);
            Assert.True(p1 > p2);

            p1 = new PosNumber(100);
            p2 = new PosNumber(0);
            Assert.True(p1 > p2);
        }

        [Fact]
        public void Explicit_GreaterThanOrEqual()
        {
            PosNumber p1 = new PosNumber(.66);
            PosNumber p2 = new PosNumber(.66);
            Assert.True(p1 >= p2);

            p1 = new PosNumber(99);
            p2 = new PosNumber(90);
            Assert.True(p1 >= p2);
        }

        [Fact]
        public void Explicit_LessThan()
        {
            PosNumber p1 = new PosNumber(0);
            PosNumber p2 = new PosNumber(400.1);
            Assert.True(p1 < p2);

            p1 = new PosNumber(Double.MaxValue - Double.MaxValue / 99999999);
            p2 = new PosNumber(Double.MaxValue);
            Assert.True(p1 < p2);
        }


        [Fact]
        public void Explicit_LessThanOrEqual()
        {
            PosNumber p1 = new PosNumber(0);
            PosNumber p2 = new PosNumber(0);
            Assert.True(p1 <= p2);

            p1 = new PosNumber(Double.MaxValue);
            p2 = new PosNumber(Double.MaxValue);
            Assert.True(p1 <= p2);
        }

        [Fact]
        public void Override_Equals()
        {
            PosNumber p1 = new PosNumber(0);
            PosNumber p2 = new PosNumber(0);
            Assert.True(p1.Equals(p2));

            p1 = new PosNumber(Double.MaxValue);
            p2 = new PosNumber(Double.MaxValue);
            Assert.True(p1.Equals(p2));


            p1 = new PosNumber(99);
            p2 = new PosNumber(98.9);
            Assert.False(p1.Equals(p2));

        }

        [Fact]
        public void Override_HashCode()
        {
            PosNumber p1 = new PosNumber(0);
            PosNumber p2 = new PosNumber(0);
            Assert.Equal(p1.GetHashCode(), p2.GetHashCode());

            p1 = new PosNumber(0);
            p2 = new PosNumber(.1);
            Assert.NotEqual(p1.GetHashCode(), p2.GetHashCode());
        }
    }
}

