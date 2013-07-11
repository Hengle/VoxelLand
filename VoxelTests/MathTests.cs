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

            Assert.AreEqual(Vector.UnitX.Normalize(), Vector.UnitX);

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
            Assert.AreEqual(Transform.Translate(1, 2, 3) * new Point(2, 2, 2), new Point(3, 4, 5));
            Assert.AreEqual(Transform.Scale(1, 2, 3) * new Point(2, 2, 2), new Point(2, 4, 6));

            var r = Transform.Rotate((float)Math.PI/2.0f, 0, 0, 1) * Vector.UnitX;
            Assert.AreEqual(r.X, Vector.UnitY.X, 0.0001f);
            Assert.AreEqual(r.Y, Vector.UnitY.Y, 0.0001f);
            Assert.AreEqual(r.Z, Vector.UnitY.Z, 0.0001f);
        }
    }
}
