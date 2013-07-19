using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VoxelLand
{
    public abstract class Camera : Entity
    {
        public Camera(Entity parent=null, CoordinateSystem coordinateSystem=null, string name="")
            : base(parent, coordinateSystem)
        {
            this.Name = name ?? String.Format("Camera #{0}", ID);
        }

        public abstract Matrix GetProjectionMatrix(Viewport viewport);
    }

    public class OrthographicCamera : Camera
    {
        public OrthographicCamera(float halfHeight=0.5f)
        {
            HalfHeight = halfHeight;
        }

        public float HalfHeight { get; set; }

        public override Matrix GetProjectionMatrix(Viewport viewport)
        {
            float aspect = viewport.Width / (float)viewport.Height;
            float halfWidth = HalfHeight * aspect;
            return Transform.Ortho(-halfWidth, halfWidth, -HalfHeight, HalfHeight, 0.1f, 100.0f);
        }
    }

    public class PerspectiveCamera : Camera
    {
        public PerspectiveCamera(float fovY=(float)Math.PI/3.0f)
        {
            VerticalFieldOfView = fovY;
        }

        public float VerticalFieldOfView { get; set; }

        public override Matrix GetProjectionMatrix(Viewport viewport)
        {
            float aspect = viewport.Width / (float)viewport.Height;
            return Transform.Perspective(VerticalFieldOfView, aspect, 0.1f, 100.0f);
        }
    }
}
