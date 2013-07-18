using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VoxelLand
{
    public static class Transform
    {
        public static Matrix Scale(float sx, float sy, float sz)
        {
            return new Matrix(new float[]
            {
                sx, 0,  0,  0,
                0,  sy, 0,  0,
                0,  0,  sz, 0,
                0,  0,  0,  1
            });
        }

        public static Matrix Scale(Vector v)
        {
            return Scale(v.X, v.Y, v.Z);
        }

        public static Matrix Translate(float dx, float dy, float dz)
        {
            return new Matrix(new float[]
            {
                1,  0,  0,  0,
                0,  1,  0,  0,
                0,  0,  1,  0,
                dx, dy, dz, 1
            });
        }

        public static Matrix Translate(Vector v)
        {
            return Translate(v.X, v.Y, v.Z);
        }

        public static Matrix Rotate(float angle, float x, float y, float z)
        {
            float c = (float)Math.Cos(angle);
            float s = (float)Math.Sin(angle);

            return new Matrix(new float[]
            {
                x*x*(1-c)+c,   y*x*(1-c)+z*s, z*x*(1-c)-y*s, 0,
                x*y*(1-c)-z*s, y*y*(1-c)+c,   z*y*(1-c)+x*s, 0,
                x*z*(1-c)+y*s, y*z*(1-c)-x*s, z*z*(1-c)+c,   0,
                0,             0,             0,             1
            });
        }

        public static Matrix Rotate(float angle, Vector axis)
        {
            return Rotate(angle, axis.X, axis.Y, axis.Z);
        }
        
        public static Matrix Frustum(float left, float right, float bottom, float top, float near, float far)
        {
            float A = (right + left) / (right - left);
            float B = (top + bottom) / (top - bottom);
            float C = - (far + near) / (far - near);
            float D = - (2 * far * near) / (far - near);

            return new Matrix(new float[16]
            {
                (2 * near) / (right - left), 0,                            0,  0,
                0,                           (2 * near) / (top - bottom),  0,  0,
                A,                           B,                            C, -1,
                0,                           0,                            D,  0
            });
        }

        public static Matrix Perspective(float fovY, float aspect, float zNear, float zFar)
        {
            float fH = (float)Math.Tan(fovY / 2.0) * zNear;
            float fW = fH * aspect;
            return Frustum(-fW, fW, -fH, fH, zNear, zFar);
        }

        public static Matrix Ortho(float left, float right, float bottom, float top, float near, float far)
        {
            float tx = - (right + left) / (right - left);
            float ty = - (top + bottom) / (top - bottom);
            float tz = - (far + near) / (far - near);

            return new Matrix(new float[] {
                2 / (right - left), 0,                   0,                0,
                0,                  2 / (top - bottom),  0,                0,
                0,                  0,                  -2 / (far - near), 0,
                tx,                 ty,                  tz,               1
            });
        }
    }
}
