using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VoxelLand
{
    public static class Transform
    {
        public static Matrix Scale(Vector s)
        {
            return new Matrix(new float[]
            {
                s.X, 0,   0,   0,
                0,   s.Y, 0,   0,
                0,   0,   s.Z, 0,
                0,   0,   0,   1
            });
        }

        public static Matrix Translate(Vector t)
        {
            return new Matrix(new float[]
            {
                1,   0,   0,   0,
                0,   1,   0,   0,
                0,   0,   1,   0,
                t.X, t.Y, t.Z, 1
            });
        }

        public static Matrix Rotate(float angle, Vector axis)
        {
            float c = (float)Math.Cos(angle);
            float s = (float)Math.Sin(angle);

            float x = axis.X;
            float y = axis.Y;
            float z = axis.Z;

            return new Matrix(new float[]
            {
                x*x*(1-c)+c,   y*x*(1-c)+z*s, z*x*(1-c)-y*s, 0,
                x*y*(1-c)-z*s, y*y*(1-c)+c,   z*y*(1-c)+x*s, 0,
                x*z*(1-c)+y*s, y*z*(1-c)-x*s, z*z*(1-c)+c,   0,
                0,             0,             0,             1
            });
        }

        public static Quaternion RotateQ(float angle, Vector axis)
        {
            axis = axis.Normalized();
            angle /= 2.0f;

        	float s = (float)Math.Sin(angle);
        	float c = (float)Math.Cos(angle);
         
            return new Quaternion(axis.X * s, axis.Y * s, axis.Z * s, c);
        }

        public static Quaternion Rotate(float pitch, float yaw, float roll)
        {
            float p = pitch / 2.0f;
            float y = yaw   / 2.0f;
            float r = roll  / 2.0f;
 
        	float sinp = (float)Math.Sin(p);
        	float siny = (float)Math.Sin(y);
        	float sinr = (float)Math.Sin(r);
        	float cosp = (float)Math.Cos(p);
        	float cosy = (float)Math.Cos(y);
        	float cosr = (float)Math.Cos(r);
 
            return new Quaternion(
                sinr * cosp * cosy - cosr * sinp * siny,
            	cosr * sinp * cosy + sinr * cosp * siny,
            	cosr * cosp * siny - sinr * sinp * cosy,
            	cosr * cosp * cosy + sinr * sinp * siny
                );
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
