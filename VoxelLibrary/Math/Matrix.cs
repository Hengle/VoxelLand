﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace VoxelLand
{
    public struct Matrix
    {
        public static Matrix Zero
        {
            get
            {
                return new Matrix(new float[]
                {
                    0, 0, 0, 0,
                    0, 0, 0, 0,
                    0, 0, 0, 0,
                    0, 0, 0, 0
                });
            }
        }

        public static Matrix Identity
        {
            get
            {
                return new Matrix(new float[]
                {
                    1, 0, 0, 0,
                    0, 1, 0, 0,
                    0, 0, 1, 0,
                    0, 0, 0, 1
                });
            }
        }

        public Matrix(float[] m)
        {
            this.m = m;
        }

        public float this[int row, int col]
        {
            get { return m[col * 4 + row]; }
            set { m[col * 4 + row] = value; }
        }

        public static implicit operator float[](Matrix m)
        {
            return m.m;
        }

        public override bool Equals(object obj)
        {
            if (! (obj is Matrix))
                return false;

            Matrix other = (Matrix)obj;

            for (int i=0; i<16; i++)
                if (this.m[i] != other.m[i])
                    return false;

            return true;
        }

        public override string ToString()
        {
            return String.Format(
                "|{0:F3}|{4:F3}|{8:F3}|{12:F3}|\n|{1:F3}|{5:F3}|{9:F3}|{13:F3}|\n|{2:F3}|{6:F3}|{10:F3}|{14:F3}|\n|{3:F3}|{7:F3}|{11:F3}|{15:F3}|",
                m[ 0], m[ 1], m[ 2], m[ 3],
                m[ 4], m[ 5], m[ 6], m[ 7],
                m[ 8], m[ 9], m[10], m[11],
                m[12], m[13], m[14], m[15]);
        }

        public Matrix Transposed()
        {
            return new Matrix(new float[]
                {
                    m[0], m[4], m[ 8], m[12],
                    m[1], m[5], m[ 9], m[13],
                    m[2], m[6], m[10], m[14],
                    m[3], m[7], m[11], m[15]
                });
        }

        public static Matrix operator*(Matrix m, float s)
        {
            return new Matrix(new float[]
            {
                m.m[0]  * s, m.m[1]  * s, m.m[2]  * s, m.m[3]  * s,
                m.m[4]  * s, m.m[5]  * s, m.m[6]  * s, m.m[7]  * s,
                m.m[8]  * s, m.m[9]  * s, m.m[10] * s, m.m[11] * s,
                m.m[12] * s, m.m[13] * s, m.m[14] * s, m.m[15] * s,
            });
        }

        public static Matrix operator/(Matrix m, float s)
        {
            return new Matrix(new float[]
            {
                m.m[0]  / s, m.m[1]  / s, m.m[2]  / s, m.m[3]  / s,
                m.m[4]  / s, m.m[5]  / s, m.m[6]  / s, m.m[7]  / s,
                m.m[8]  / s, m.m[9]  / s, m.m[10] / s, m.m[11] / s,
                m.m[12] / s, m.m[13] / s, m.m[14] / s, m.m[15] / s,
            });
        }

        public static Matrix operator+(Matrix m1, Matrix m2)
        {
            return new Matrix(new float[]
            {
                m1.m[ 0]+m2.m[ 0], m1.m[ 1]+m2.m[ 1], m1.m[ 2]+m2.m[ 2], m1.m[ 3]+m2.m[ 3],
                m1.m[ 4]+m2.m[ 4], m1.m[ 5]+m2.m[ 5], m1.m[ 6]+m2.m[ 6], m1.m[ 7]+m2.m[ 7],
                m1.m[ 8]+m2.m[ 8], m1.m[ 9]+m2.m[ 9], m1.m[10]+m2.m[10], m1.m[11]+m2.m[11],
                m1.m[12]+m2.m[12], m1.m[13]+m2.m[13], m1.m[14]+m2.m[14], m1.m[15]+m2.m[15],
            });
        }

        public static Matrix operator-(Matrix m1, Matrix m2)
        {
            return new Matrix(new float[]
            {
                m1.m[ 0]-m2.m[ 0], m1.m[ 1]-m2.m[ 1], m1.m[ 2]-m2.m[ 2], m1.m[ 3]-m2.m[ 3],
                m1.m[ 4]-m2.m[ 4], m1.m[ 5]-m2.m[ 5], m1.m[ 6]-m2.m[ 6], m1.m[ 7]-m2.m[ 7],
                m1.m[ 8]-m2.m[ 8], m1.m[ 9]-m2.m[ 9], m1.m[10]-m2.m[10], m1.m[11]-m2.m[11],
                m1.m[12]-m2.m[12], m1.m[13]-m2.m[13], m1.m[14]-m2.m[14], m1.m[15]-m2.m[15],
            });
        }

        public static Matrix operator*(Matrix m1, Matrix m2)
        {
            return new Matrix(new float[]
            {
                m1[0,0]*m2[0,0] + m1[0,1]*m2[1,0] + m1[0,2]*m2[2,0] + m1[0,3]*m2[3,0],
                m1[1,0]*m2[0,0] + m1[1,1]*m2[1,0] + m1[1,2]*m2[2,0] + m1[1,3]*m2[3,0],
                m1[2,0]*m2[0,0] + m1[2,1]*m2[1,0] + m1[2,2]*m2[2,0] + m1[2,3]*m2[3,0],
                m1[3,0]*m2[0,0] + m1[3,1]*m2[1,0] + m1[3,2]*m2[2,0] + m1[3,3]*m2[3,0],

                m1[0,0]*m2[0,1] + m1[0,1]*m2[1,1] + m1[0,2]*m2[2,1] + m1[0,3]*m2[3,1],
                m1[1,0]*m2[0,1] + m1[1,1]*m2[1,1] + m1[1,2]*m2[2,1] + m1[1,3]*m2[3,1],
                m1[2,0]*m2[0,1] + m1[2,1]*m2[1,1] + m1[2,2]*m2[2,1] + m1[2,3]*m2[3,1],
                m1[3,0]*m2[0,1] + m1[3,1]*m2[1,1] + m1[3,2]*m2[2,1] + m1[3,3]*m2[3,1],

                m1[0,0]*m2[0,2] + m1[0,1]*m2[1,2] + m1[0,2]*m2[2,2] + m1[0,3]*m2[3,2],
                m1[1,0]*m2[0,2] + m1[1,1]*m2[1,2] + m1[1,2]*m2[2,2] + m1[1,3]*m2[3,2],
                m1[2,0]*m2[0,2] + m1[2,1]*m2[1,2] + m1[2,2]*m2[2,2] + m1[2,3]*m2[3,2],
                m1[3,0]*m2[0,2] + m1[3,1]*m2[1,2] + m1[3,2]*m2[2,2] + m1[3,3]*m2[3,2],

                m1[0,0]*m2[0,3] + m1[0,1]*m2[1,3] + m1[0,2]*m2[2,3] + m1[0,3]*m2[3,3],
                m1[1,0]*m2[0,3] + m1[1,1]*m2[1,3] + m1[1,2]*m2[2,3] + m1[1,3]*m2[3,3],
                m1[2,0]*m2[0,3] + m1[2,1]*m2[1,3] + m1[2,2]*m2[2,3] + m1[2,3]*m2[3,3],
                m1[3,0]*m2[0,3] + m1[3,1]*m2[1,3] + m1[3,2]*m2[2,3] + m1[3,3]*m2[3,3],
            });
        }

        public static Vector operator*(Matrix m, Vector v)
        {
            return new Vector(
                m[0,0]*v.X + m[0,1]*v.Y + m[0,2]*v.Z,
                m[1,0]*v.X + m[1,1]*v.Y + m[1,2]*v.Z,
                m[2,0]*v.X + m[2,1]*v.Y + m[2,2]*v.Z
            );
        }

        public static Point operator*(Matrix m, Point p)
        {
            return new Point(
                m[0,0]*p.X + m[0,1]*p.Y + m[0,2]*p.Z + m[0,3],
                m[1,0]*p.X + m[1,1]*p.Y + m[1,2]*p.Z + m[1,3],
                m[2,0]*p.X + m[2,1]*p.Y + m[2,2]*p.Z + m[2,3],
                m[3,0]*p.X + m[3,1]*p.Y + m[3,2]*p.Z + m[3,3]
            );
        }

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType=UnmanagedType.R4, SizeConst=16)]
        private float[] m;
    }
}
