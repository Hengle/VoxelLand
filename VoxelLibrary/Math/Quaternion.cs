using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace VoxelLand
{
    public struct Quaternion
    {
        public Quaternion(float x, float y, float z, float w)
        {
            q = new float[] { x, y, z, w };
            float m = MagnitudeSquared;
            X /= m;
            Y /= m;
            Z /= m;
            W /= m;
        }

        public static implicit operator float[](Quaternion q)
        {
            return q.q;
        }
        
        public static implicit operator Matrix(Quaternion q)
        {
        	float x2 = q.X * q.X;
        	float y2 = q.Y * q.Y;
        	float z2 = q.Z * q.Z;
        	float xy = q.X * q.Y;
        	float xz = q.X * q.Z;
        	float yz = q.Y * q.Z;
        	float wx = q.W * q.X;
        	float wy = q.W * q.Y;
        	float wz = q.W * q.Z;
         
        	return new Matrix(new float[] {
                1.0f - 2.0f * (y2 + z2), 2.0f * (xy - wz),        2.0f * (xz + wy),        0.0f,
        		2.0f * (xy + wz),        1.0f - 2.0f * (x2 + z2), 2.0f * (yz - wx),        0.0f,
        		2.0f * (xz - wy),        2.0f * (yz + wx),        1.0f - 2.0f * (x2 + y2), 0.0f,
        		0.0f,                    0.0f,                    0.0f,                    1.0f
            });
        }

        public override bool Equals(object obj)
        {
            if (! (obj is Quaternion))
                return false;

            Quaternion other = (Quaternion)obj;

            for (int i=0; i<4; i++)
                if (this.q[i] != other.q[i])
                    return false;

            return true;
        }

        public float Magnitude
        {
            get { return (float)Math.Sqrt(MagnitudeSquared); }
        }

        public float MagnitudeSquared
        {
            get { return X*X + Y+Y + Z*Z + W*W; }
        }

        public Quaternion Conjugate()
        {
            return new Quaternion(-X, -Y, -Z, W);
        }

        public static Quaternion operator*(Quaternion q, Quaternion r)
        {
            return new Quaternion(
                q.W*r.X + q.X*r.W + q.Y*r.Z - q.Z*r.Y,
                q.W*r.Y + q.Y*r.W + q.Z*r.X - q.X*r.Z,
                q.W*r.Z + q.Z*r.W + q.X*r.Y - q.Y*r.X,
                q.W*r.W - q.X*r.X - q.Y*r.Y - q.Z*r.Z);
        }

        public static Vector operator*(Quaternion q, Vector v)
        {
            Vector vn = v.Normalized();
            var r = q * (new Quaternion(vn.X, vn.Y, vn.Z, 0.0f) * q.Conjugate());
            return new Vector(r.X, r.Y, r.Z);
        }

        public override string ToString()
        {
            return String.Format("<{0:F3}, {1:F3}, {2:F3}, {3:F3}>", X, Y, Z, W);
        }

        public float this[int component]
        {
            get { return q[component]; }
        }

        public float X { get { return q[0]; } set { q[0] = value; } }
        public float Y { get { return q[1]; } set { q[1] = value; } }
        public float Z { get { return q[2]; } set { q[2] = value; } }
        public float W { get { return q[3]; } set { q[3] = value; } }

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType=UnmanagedType.R4, SizeConst=4)]
        private float[] q;
    }
}
