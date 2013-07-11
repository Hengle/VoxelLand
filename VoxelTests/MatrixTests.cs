using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using VoxelLand;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VoxelTests
{
    [TestClass]
    public class MatrixTests
    {
        public MatrixTests()
        {
        }

        [TestMethod]
        public void TestMethod1()
        {
            Matrix m = new Matrix(new float[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 });
            Assert.AreEqual(m * Matrix.Identity, m);
        }
    }
}
