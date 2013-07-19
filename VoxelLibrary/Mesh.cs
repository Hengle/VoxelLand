using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using SharpGL;

namespace VoxelLand
{
    public class Mesh
    {
        public Mesh(OpenGL gl)
        {
            this.gl = gl;
            this.bufferCount = 0;

            uint[] arrs = new uint[1];
            gl.GenVertexArrays(1, arrs);
            ID = arrs[0];
        }

        public uint ID { get; private set; }

        public void AddBuffer(Buffer<Point> buffer)
        {
            uint index = bufferCount++;

            gl.BindVertexArray(ID);
            gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, buffer.ID);
            gl.VertexAttribPointer(index, 4, OpenGL.GL_FLOAT, false, 0, IntPtr.Zero);
            gl.EnableVertexAttribArray(index);
        }

        public void AddBuffer(Buffer<Vector> buffer)
        {
            uint index = bufferCount++;

            gl.BindVertexArray(ID);
            gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, buffer.ID);
            gl.VertexAttribPointer(index, 4, OpenGL.GL_FLOAT, false, 0, IntPtr.Zero);
            gl.EnableVertexAttribArray(index);
        }

        public void AddBuffer(Buffer<Matrix> buffer)
        {
            uint index = bufferCount++;

            gl.BindVertexArray(ID);
            gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, buffer.ID);
            gl.VertexAttribPointer(index, 4, OpenGL.GL_FLOAT, false, 0, IntPtr.Zero);
            gl.EnableVertexAttribArray(index);
        }

        private OpenGL gl;
        private uint bufferCount;
    }
}
