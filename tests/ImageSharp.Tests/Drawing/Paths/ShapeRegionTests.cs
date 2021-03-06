﻿
namespace ImageSharp.Tests.Drawing.Paths
{
    using System;
    using System.IO;
    using ImageSharp;
    using ImageSharp.Drawing.Brushes;
    using ImageSharp.Processing;
    using System.Collections.Generic;
    using Xunit;
    using ImageSharp.Drawing;
    using System.Numerics;
    using SixLabors.Shapes;
    using ImageSharp.Drawing.Processors;
    using ImageSharp.Drawing.Pens;
    using Moq;
    using System.Collections.Immutable;
    using SixLabors.Primitives;

    public class ShapeRegionTests 
    {
        private readonly Mock<IPath> pathMock;
        private readonly SixLabors.Primitives.RectangleF bounds;

        public ShapeRegionTests()
        {
            this.pathMock = new Mock<IPath>();

            this.bounds = new RectangleF(10.5f, 10, 10, 10);
            pathMock.Setup(x => x.Bounds).Returns(this.bounds);
            // wire up the 2 mocks to reference eachother
            pathMock.Setup(x => x.AsClosedPath()).Returns(() => pathMock.Object);
        }

        [Fact]
        public void ShapeRegionWithPathCallsAsShape()
        {
            new ShapeRegion(pathMock.Object);

            pathMock.Verify(x => x.AsClosedPath());
        }

        [Fact]
        public void ShapeRegionWithPathRetainsShape()
        {
            ShapeRegion region = new ShapeRegion(pathMock.Object);

            Assert.Equal(pathMock.Object, region.Shape);
        }

        [Fact]
        public void ShapeRegionFromPathConvertsBoundsProxyToShape()
        {
            ShapeRegion region = new ShapeRegion(pathMock.Object);

            Assert.Equal(Math.Floor(bounds.Left), region.Bounds.Left);
            Assert.Equal(Math.Ceiling(bounds.Right), region.Bounds.Right);

            pathMock.Verify(x => x.Bounds);
        }

        [Fact]
        public void ShapeRegionFromPathMaxIntersectionsProxyToShape()
        {
            ShapeRegion region = new ShapeRegion(pathMock.Object);

            int i = region.MaxIntersections;
            pathMock.Verify(x => x.MaxIntersections);
        }

        [Fact]
        public void ShapeRegionFromPathScanYProxyToShape()
        {
            int yToScan = 10;
            ShapeRegion region = new ShapeRegion(pathMock.Object);

            pathMock.Setup(x => x.FindIntersections(It.IsAny<PointF>(), It.IsAny<PointF>(), It.IsAny<Span<PointF>>()))
                .Callback<PointF, PointF, Span<PointF>>((s, e, b) => {
                    Assert.Equal(yToScan, s.Y);
                    Assert.Equal(yToScan, e.Y);
                    Assert.True(s.X < bounds.Left);
                    Assert.True(e.X > bounds.Right);
                }).Returns(0);

            int i = region.Scan(yToScan, new float[0]);

            pathMock.Verify(x => x.FindIntersections(It.IsAny<PointF>(), It.IsAny<PointF>(), It.IsAny<Span<PointF>>()), Times.Once);
        }

        [Fact]
        public void ShapeRegionFromShapeScanYProxyToShape()
        {
            int yToScan = 10;
            ShapeRegion region = new ShapeRegion(pathMock.Object);

            pathMock.Setup(x => x.FindIntersections(It.IsAny<PointF>(), It.IsAny<PointF>(), It.IsAny<Span<PointF>>()))
                .Callback<PointF, PointF, Span<PointF>>((s, e, b) => {
                    Assert.Equal(yToScan, s.Y);
                    Assert.Equal(yToScan, e.Y);
                    Assert.True(s.X < bounds.Left);
                    Assert.True(e.X > bounds.Right);
                }).Returns(0);

            int i = region.Scan(yToScan, new float[0]);

            pathMock.Verify(x => x.FindIntersections(It.IsAny<PointF>(), It.IsAny<PointF>(), It.IsAny<Span<PointF>>()), Times.Once);
        }

        [Fact]
        public void ShapeRegionFromShapeConvertsBoundsProxyToShape()
        {
            ShapeRegion region = new ShapeRegion(pathMock.Object);

            Assert.Equal(Math.Floor(bounds.Left), region.Bounds.Left);
            Assert.Equal(Math.Ceiling(bounds.Right), region.Bounds.Right);

            pathMock.Verify(x => x.Bounds);
        }

        [Fact]
        public void ShapeRegionFromShapeMaxIntersectionsProxyToShape()
        {
            ShapeRegion region = new ShapeRegion(pathMock.Object);

            int i = region.MaxIntersections;
            pathMock.Verify(x => x.MaxIntersections);
        }
    }
}
