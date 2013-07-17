using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VoxelLand
{
    public class Viewport
    {
        public Viewport(int x, int y, int w, int h)
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
        }

        public int Left { get { return x; } }
        public int Top { get { return y; } }
        public int Right { get { return x + w; } }
        public int Bottom { get { return y + h; } }
        public int Width { get { return w; } }
        public int Height { get { return h; } }

        private int x, y, w, h;
    }
}
