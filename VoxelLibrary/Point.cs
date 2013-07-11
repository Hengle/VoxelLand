using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VoxelLand
{
    public class Point
    {
        public static Point Origin { get { return new Point(0, 0, 0); } }

        public Point()
        {
            p = new float[] { 0, 0, 0, 0 };
        }

        public Point(float x, float y, float z)
        {
            p = new float[] { x, y, z, 1 };
        }

        public Point(float x, float y, float z, float w)
        {
            p = new float[] { x/w, y/w, z/w, 1 };
        }

        public static implicit operator float[](Point p)
        {
            return p.p;
        }

        public override bool Equals(object obj)
        {
            if (! (obj is Point))
                return false;

            Point other = (Point)obj;

            for (int i=0; i<4; i++)
                if (this.p[i] != other.p[i])
                    return false;

            return true;
        }

        public override string ToString()
        {
            return String.Format("({0:F3}, {1:F3}, {2:F3})", X, Y, Z);
        }

        public float this[int component]
        {
            get { return p[component]; }
        }

        public float X { get { return p[0]; } set { p[0] = value; } }
        public float Y { get { return p[1]; } set { p[1] = value; } }
        public float Z { get { return p[2]; } set { p[2] = value; } }

        public static Vector operator-(Point p, Point q)
        {
            return new Vector(p.X-q.X, p.Y-q.Y, p.Z-q.Z);
        }

        public static Point operator+(Point p, Vector v)
        {
            return new Point(p.X+v.X, p.Y+v.Y, p.Z+v.Z);
        }

        public static Point operator-(Point p, Vector v)
        {
            return new Point(p.X-v.X, p.Y-v.Y, p.Z-v.Z);
        }

        private float[] p;
    }
}
