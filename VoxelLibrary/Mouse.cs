using System;
using System.Threading;

namespace VoxelLand
{
    public class Mouse
    {
        public Mouse()
        {
            inputLock = new object();
            delta = Vector.Zero;
        }

        public void OnMouseMove(float dx, float dy)
        {
            lock (inputLock)
            {
                delta.X += dx;
                delta.Y += dy;
            }
        }

        public Vector Peek()
        {
            return delta;
        }

        public Vector Read()
        {
            lock (inputLock)
            {
                Vector result = delta;
                delta = new Vector(0.0f, 0.0f, 0.0f);
                return result;
            }
        }

        private object inputLock;
        private Vector delta;
    }
}
