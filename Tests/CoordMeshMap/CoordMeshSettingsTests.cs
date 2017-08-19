using Quote.CoordMeshMap;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CoordMeshMap.Tests
{
    public class CoordMeshSettingsTests
    {
        [Fact]
        public void CoordMeshSettigns_AreValid()
        {
            var settings = new CoordMeshSettings(.8, 1, 10, 5, 6);
            Assert.True(settings.SettingsAreValid());
            Assert.Equal(.8, (double)settings.RESOLUTION);
            Assert.Equal(1, (int)settings.LON_MIN);
            Assert.Equal(10, (int)settings.LON_MAX);
            Assert.Equal(5, (int)settings.LAT_MIN);
            Assert.Equal(6, (int)settings.LAT_MAX);

        }

        [Fact]
        public void CoordMeshSettigns_AreNotValid_ThrowsArgumentException()
        {
            try
            {
                var settings = new CoordMeshSettings(.8, 1, 0, 5, 6);
            }
            catch(ArgumentException)
            {
                Assert.True(true);
            }
        }

        [Fact]
        public void CoordMeshSettigns_AreNotValid_ThrowsArgumentOutOfRangeException()
        {
            try
            {
                var settings = new CoordMeshSettings(-0.8, 1, 10, 5, 6);
            }
            catch (ArgumentOutOfRangeException)
            {
                Assert.True(true);
            }
        }
    }
}
