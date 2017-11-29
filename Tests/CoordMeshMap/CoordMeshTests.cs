using Quote.CoordMeshMap;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CoordMeshMap.Tests
{
    public class CoordMeshTests
    {

        private CoordMesh CreateMesh_ResolutionLessThan1_ConstrainHandler()
        {
            return new CoordMesh(new CoordMeshSettings(.1, 1, 2, 40, 50), BoundaryHandler.HandleBoundary_ConstrainOutOfBounds);
        }

        [Fact]
        public void ResolutionLessThan1_ConstrainHandler_ConstrainLeft()
        {
            var mesh = CreateMesh_ResolutionLessThan1_ConstrainHandler();
            var handler = mesh.FitLon(.9);
            Assert.True(handler.Ok);
            Assert.Equal(0, handler.Coord.AsInt);
        }
        [Fact]
        public void ResolutionLessThan1_ConstrainHandler_ConstrainRight()
        {
            var mesh = CreateMesh_ResolutionLessThan1_ConstrainHandler();
            var handler = mesh.FitLon(3);
            Assert.True(handler.Ok);
            Assert.Equal(2, handler.Coord.AsInt);
        }

    }
}
