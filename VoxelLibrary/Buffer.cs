using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using SharpGL;

namespace VoxelLand
{
    public class Buffer<T> : IEnumerable<T> where T : struct
    {
        public Buffer(OpenGL gl, params T[] items)
        {
            Count = items.Length;
            elementSize = Marshal.SizeOf(typeof(T));

            buffer = Marshal.AllocHGlobal(elementSize * Count);
            for (int i = 0; i < Count; i++)
                Marshal.StructureToPtr(items[i], IntPtr.Add(buffer, i * elementSize), false);

            uint[] bufs = new uint[1];
            gl.GenBuffers(1, bufs);
            gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, bufs[0]);
            gl.BufferData(OpenGL.GL_ARRAY_BUFFER, elementSize * Count, buffer, OpenGL.GL_STATIC_DRAW);

            ID = bufs[0];
        }

        public uint ID { get; private set; }

        public int Count { get; private set; }

        public T this[int idx]
        {
            get
            {
                if (idx >= Count)
                    throw new IndexOutOfRangeException();

                return (T)Marshal.PtrToStructure(IntPtr.Add(buffer, idx * elementSize), typeof(T));
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i=0; i<Count; i++)
                yield return this[i];
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IntPtr buffer;
        private int elementSize;
    }
}
