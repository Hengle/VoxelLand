using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VoxelLand
{
    public class Vector
    {
        public static Vector Zero  { get { return new Vector(0, 0, 0); } }
        public static Vector UnitX { get { return new Vector(1, 0, 0); } }
        public static Vector UnitY { get { return new Vector(0, 1, 0); } }
        public static Vector UnitZ { get { return new Vector(0, 0, 1); } }

        public Vector()
        {
            v = new float[] { 0, 0, 0, 0 };
        }

        public Vector(float x, float y, float z)
        {
            v = new float[] { x, y, z, 0 };
        }

        public static implicit operator float[](Vector v)
        {
            return v.v;
        }

        public override bool Equals(object obj)
        {
            if (! (obj is Vector))
                return false;

            Vector other = (Vector)obj;

            for (int i=0; i<4; i++)
                if (this.v[i] != other.v[i])
                    return false;

            return true;
        }

        public override string ToString()
        {
            return String.Format("<{0:F3}, {1:F3}, {2:F3}>", X, Y, Z);
        }

        public float this[int component]
        {
            get { return v[component]; }
        }

        public float X { get { return v[0]; } set { v[0] = value; } }
        public float Y { get { return v[1]; } set { v[1] = value; } }
        public float Z { get { return v[2]; } set { v[2] = value; } }

        public static Vector operator*(Vector v, float s)
        {
            return new Vector(v.X*s, v.Y*s, v.Z*s);
        }

        public static Vector operator/(Vector v, float s)
        {
            return new Vector(v.X/s, v.Y/s, v.Z/s);
        }

        public static Vector operator+(Vector u, Vector v)
        {
            return new Vector(u.X+v.X, u.Y+v.Y, u.Z+v.Z);
        }

        public static Vector operator-(Vector u, Vector v)
        {
            return new Vector(u.X-v.X, u.Y-v.Y, u.Z-v.Z);
        }

        public static float Dot(Vector u, Vector v)
        {
            return u.X*v.X + u.Y*v.Y + u.Z*v.Z;
        }

        public static Vector Cross(Vector u, Vector v)
        {
            return new Vector(
                u.Y*v.Z-u.Z*v.Y,
                u.Z*v.X-u.X*v.Z,
                u.X*v.Y-u.Y*v.X
            );
        }

        public Vector Normalize()
        {
            return this / Length;
        }

        public float LengthSquared
        {
            get
            {
                return X*X + Y*Y + Z*Z;
            }
        }

        public float Length
        {
            get
            {
                return (float)Math.Sqrt(LengthSquared);
            }
        }

        private float[] v;
    }
}
