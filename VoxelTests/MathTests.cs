using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using VoxelLand;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VoxelTests
{
    [TestClass]
    public class MathTests
    {
        public MathTests()
        {
        }

        [TestMethod]
        public void TestMatrix()
        {
            var m = new Matrix(Enumerable.Range(1, 16).Select(i => (float) i     ).ToArray());
            var r = new Matrix(Enumerable.Range(1, 16).Select(i => (float)(i * 2)).ToArray());

            Assert.AreEqual(m * 2.0f, r);
            Assert.AreEqual(r * 0.5f, m);

            Assert.AreEqual(m / 0.5f, r);
            Assert.AreEqual(r / 2.0f, m);

            Assert.AreEqual(m + m, r);
            Assert.AreEqual(r - m, m);

            Assert.AreEqual(m * Matrix.Identity, m);
            Assert.AreEqual(m * Matrix.Zero,     Matrix.Zero);

            Assert.AreEqual(Matrix.Identity * m, m);
            Assert.AreEqual(Matrix.Zero     * m, Matrix.Zero);
        }

        [TestMethod]
        public void TestVector()
        {
            var v = Vector.UnitX + Vector.UnitY / 0.5f - Vector.UnitZ * -3.0f;

            Assert.AreEqual(v, new Vector(1, 2, 3));

            Assert.AreEqual(Vector.Dot(Vector.UnitX, Vector.UnitX),  1.0f);
            Assert.AreEqual(Vector.Dot(Vector.UnitX, Vector.UnitY),  0.0f);
            Assert.AreEqual(Vector.Dot(v,            v),            14.0f);

            Assert.AreEqual(Vector.Cross(Vector.UnitX, Vector.UnitY), Vector.UnitZ);

            Assert.AreEqual(Vector.UnitX.Normalized(), Vector.UnitX);

            Assert.AreEqual(Vector.UnitX.Length, 1.0f);
            Assert.AreEqual(v.LengthSquared, 14.0f);
        }

        [TestMethod]
        public void TestPoint()
        {
            Assert.AreEqual(new Point(1, 2, 3) - Point.Origin, new Vector(1, 2, 3));
            Assert.AreEqual(new Point(1, 2, 3) - new Point(4, 5, 6), new Vector(-3, -3, -3));
            Assert.AreEqual(new Point(1, 2, 3) + Vector.UnitX, new Point(2, 2, 3));
            Assert.AreEqual(new Point(1, 2, 3) + new Vector(3, 2, 1), new Point(4, 4, 4));
            Assert.AreEqual(new Point(1, 2, 3) - new Vector(3, 2, 1), new Point(-2, 0, 2));
        }

        [TestMethod]
        public void TestTransform()
        {
            Assert.AreEqual(Transform.Translate(new Vector(1, 2, 3)) * new Point(2, 2, 2), new Point(3, 4, 5));
            Assert.AreEqual(Transform.Scale(new Vector(1, 2, 3)) * new Point(2, 2, 2), new Point(2, 4, 6));
            
            AssertVectorsAreEqual(Transform.Rotate((float)Math.PI/2.0f, Vector.UnitZ) * Vector.UnitX, Vector.UnitY);
        }

        [TestMethod]
        public void TestDefaultCoordinateSystem()
        {
            var c = CoordinateSystem.Default;
            AssertPointsAreEqual(c.ToLocal(Point.Origin), Point.Origin);
            AssertPointsAreEqual(c.ToGlobal(Point.Origin), Point.Origin);
        }

        [TestMethod]
        public void TestTranslatedCoordinateSystem()
        {
            var c = CoordinateSystem.Default.GloballyTranslated(new Vector(1, 2, 3));
            AssertPointsAreEqual(c.ToLocal(Point.Origin), new Point(-1, -2, -3));
            AssertPointsAreEqual(c.ToGlobal(Point.Origin), new Point(1, 2, 3));
        }

        [TestMethod]
        public void TestLocallyRotatedCoordinateSystem()
        {
            var c = CoordinateSystem.Default.LocallyRotated((float)Math.PI, Vector.UnitX);
            AssertPointsAreEqual(c.ToLocal(new Point(0, 0, -1)), new Point(0, 0, 1));
            AssertPointsAreEqual(c.ToLocal(new Point(0, 1, 0)), new Point(0, -1, 0));
            AssertPointsAreEqual(c.ToLocal(new Point(1, 0, 0)), new Point(1, 0, 0));
            AssertPointsAreEqual(new Point(0, 0, -1), c.ToGlobal(new Point(0, 0, 1)));
            AssertPointsAreEqual(new Point(0, 1, 0),  c.ToGlobal(new Point(0, -1, 0)));
            AssertPointsAreEqual(new Point(1, 0, 0),  c.ToGlobal(new Point(1, 0, 0)));
        }

        [TestMethod]
        public void TestGloballyRotatedCoordinateSystem()
        {
            var c = CoordinateSystem.Default.GloballyTranslated(new Vector(1, 0, 0)).GloballyRotated((float)Math.PI/2.0f, Vector.UnitY);
            AssertPointsAreEqual(c.ToLocal(Point.Origin), new Point(-1, 0, 0));
            AssertPointsAreEqual(c.ToGlobal(Point.Origin), new Point(0, 0, -1));
        }

        [TestMethod]
        public void TestSimpleCoordinateSystem()
        {
            var c = CoordinateSystem.Default.GloballyTranslated(new Vector(1, 0, 0)).LocallyRotated((float)Math.PI/2.0f, Vector.UnitY);
            AssertPointsAreEqual(c.ToLocal(Point.Origin), new Point(0, 0, -1));
            AssertPointsAreEqual(c.ToGlobal(Point.Origin), new Point(1, 0, 0));
        }

        [TestMethod]
        public void TestComplexCoordinateSystem()
        {
            var c =
                CoordinateSystem
                    .Default
                    .LocallyRotated((float)Math.PI/-2.0f, Vector.UnitZ)
                    .LocallyTranslated(Vector.UnitX)
                    .GloballyTranslated(Vector.UnitZ)
                    .GloballyRotated((float)Math.PI, Vector.UnitX)
                    .LocallyRotated((float)Math.PI, Vector.UnitY)
                    .LocallyTranslated(Vector.UnitZ)
                    .GloballyTranslated(Vector.UnitY * -1)
                    .LocallyRotated((float)Math.PI/2.0f, Vector.UnitZ);

            AssertPointsAreEqual(c.ToLocal(Point.Origin), Point.Origin);
            AssertPointsAreEqual(c.ToGlobal(Point.Origin), Point.Origin);

            AssertPointsAreEqual(c.ToLocal(Point.Origin + Vector.UnitX), Point.Origin + Vector.UnitX);
            AssertPointsAreEqual(c.ToGlobal(Point.Origin + Vector.UnitX), Point.Origin + Vector.UnitX);

            AssertPointsAreEqual(c.ToLocal(Point.Origin + Vector.UnitY), Point.Origin + Vector.UnitY);
            AssertPointsAreEqual(c.ToGlobal(Point.Origin + Vector.UnitY), Point.Origin + Vector.UnitY);

            AssertPointsAreEqual(c.ToLocal(Point.Origin + Vector.UnitZ), Point.Origin + Vector.UnitZ);
            AssertPointsAreEqual(c.ToGlobal(Point.Origin + Vector.UnitZ), Point.Origin + Vector.UnitZ);
        }


        private static void AssertVectorsAreEqual(Vector u, Vector v)
        {
            Assert.AreEqual(u.X, v.X, 0.0001);
            Assert.AreEqual(u.Y, v.Y, 0.0001);
            Assert.AreEqual(u.Z, v.Z, 0.0001);
        }

        private static void AssertPointsAreEqual(Point p, Point q)
        {
            Assert.AreEqual(p.X, q.X, 0.0001);
            Assert.AreEqual(p.Y, q.Y, 0.0001);
            Assert.AreEqual(p.Z, q.Z, 0.0001);
        }
    }
}
