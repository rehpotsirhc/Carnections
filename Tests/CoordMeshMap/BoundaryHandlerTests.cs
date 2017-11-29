using Quote.CoordMeshMap;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CoordMeshMap.Tests
{
    public class BoundaryHandlerTests
    {

        [Fact]
        public void Zero_Okay()
        {
            var boundaryHandler = new BoundaryHandler(0);
            Assert.True(boundaryHandler.Ok);
            Assert.Equal(0, boundaryHandler.Coord.AsInt);
        }

        [Fact]
        public void Positive_Okay()
        {
            var boundaryHandler = new BoundaryHandler(60);
            Assert.True(boundaryHandler.Ok);
            Assert.Equal(60, boundaryHandler.Coord.AsInt);
        }

        [Fact]
        public void Negative_NotOkay()
        {
            var boundaryHandler = new BoundaryHandler(-1);
            Assert.False(boundaryHandler.Ok);
            Assert.Null(boundaryHandler.Coord);
        }

        [Fact]
        public void HandleBoundary_Constrain_CoordGreaterThanlength_PushDown()
        {
            var boundaryHandler = BoundaryHandler.HandleBoundary_ConstrainOutOfBounds(5, new PosInt(4));
            Assert.True(boundaryHandler.Ok);
            Assert.Equal(3, boundaryHandler.Coord.AsInt);
        }

        [Fact]
        public void HandleBoundary_Constrain_CoordEqualToLength_PushDown()
        {
            var boundaryHandler = BoundaryHandler.HandleBoundary_ConstrainOutOfBounds(11, new PosInt(11));
            Assert.True(boundaryHandler.Ok);
            Assert.Equal(10, boundaryHandler.Coord.AsInt);
        }

        [Fact]
        public void HandleBoundary_Constrain_LessThanZero_PushUpToZero()
        {
            var boundaryHandler = BoundaryHandler.HandleBoundary_ConstrainOutOfBounds(-1, new PosInt(11));
            Assert.True(boundaryHandler.Ok);
            Assert.Equal(0, boundaryHandler.Coord.AsInt);
        }

        [Fact]
        public void HandleBoundary_Constrain_Zero_NoChange()
        {
            var boundaryHandler = BoundaryHandler.HandleBoundary_ConstrainOutOfBounds(0, new PosInt(121));
            Assert.True(boundaryHandler.Ok);
            Assert.Equal(0, boundaryHandler.Coord.AsInt);
        }

        [Fact]
        public void HandleBoundary_Constrain_InBetween_NoChange()
        {
            var boundaryHandler = BoundaryHandler.HandleBoundary_ConstrainOutOfBounds(120, new PosInt(121));
            Assert.True(boundaryHandler.Ok);
            Assert.Equal(120, boundaryHandler.Coord.AsInt);
        }

        [Fact]
        public void HandleBoundary_IgnoreOutOfBounds_IsOutOfBounds_NotOkay_1()
        {
            var boundaryHandler = BoundaryHandler.HandleBoundary_IgnoreOutOfBounds(4564, 1000);
            Assert.False(boundaryHandler.Ok);
        }

        [Fact]
        public void HandleBoundary_IgnoreOutOfBounds_IsOutOfBounds_NotOkay_2()
        {
            var boundaryHandler = BoundaryHandler.HandleBoundary_IgnoreOutOfBounds(-5000, 1000);
            Assert.False(boundaryHandler.Ok);

        }

        [Fact]
        public void HandleBoundary_IgnoreOutOfBounds_InBounds_Okay_1()
        {
            var boundaryHandler = BoundaryHandler.HandleBoundary_IgnoreOutOfBounds(5, 1000);
            Assert.True(boundaryHandler.Ok);
        }

        [Fact]
        public void HandleBoundary_IgnoreOutOfBounds_InBounds_Okay_2()
        {
            var boundaryHandler = BoundaryHandler.HandleBoundary_IgnoreOutOfBounds(999, 1000);
            Assert.True(boundaryHandler.Ok);
        }
    }
}
