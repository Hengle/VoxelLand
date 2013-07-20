using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using SharpGL;

namespace VoxelLand
{
    public enum MeshType : uint
    {
        Points        = OpenGL.GL_POINTS,
        Lines         = OpenGL.GL_LINES,
        LineStrip     = OpenGL.GL_LINE_STRIP,
        LineLoop      = OpenGL.GL_LINE_LOOP,
        Triangles     = OpenGL.GL_TRIANGLES,
        TriangleStrip = OpenGL.GL_TRIANGLE_STRIP,
        TriangleFan   = OpenGL.GL_TRIANGLE_FAN,
    }

    public class Mesh
    {
        public Mesh(OpenGL gl, MeshType type)
        {
            this.gl = gl;
            this.bufferCount = 0;

            Type = type;

            uint[] arrs = new uint[1];
            gl.GenVertexArrays(1, arrs);
            ID = arrs[0];
        }

        public uint ID { get; private set; }

        public MeshType Type { get; private set; }

        public int Length { get; private set; }

        public void AddBuffer(Buffer<Point> buffer)
        {
            Length = buffer.Count;

            uint index = bufferCount++;

            gl.BindVertexArray(ID);
            gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, buffer.ID);
            gl.VertexAttribPointer(index, 4, OpenGL.GL_FLOAT, false, 0, IntPtr.Zero);
            gl.EnableVertexAttribArray(index);
        }

        public void AddBuffer(Buffer<Vector> buffer)
        {
            Length = buffer.Count;

            uint index = bufferCount++;

            gl.BindVertexArray(ID);
            gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, buffer.ID);
            gl.VertexAttribPointer(index, 4, OpenGL.GL_FLOAT, false, 0, IntPtr.Zero);
            gl.EnableVertexAttribArray(index);
        }

        public void AddBuffer(Buffer<Matrix> buffer)
        {
            Length = buffer.Count;

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
