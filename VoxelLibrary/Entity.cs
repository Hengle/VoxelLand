using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VoxelLand
{
    public abstract class Entity
    {
        public Entity(CoordinateSystem coordinateSystem=null, string name="")
        {
            ID = NextID++;
            CoordinateSystem = coordinateSystem ?? CoordinateSystem.Default;
            Name = name.Length > 0 ? name : String.Format("Entity #{0}", ID);
        }

        public long ID { get; private set; }

        public string Name { get; set; }

        public CoordinateSystem CoordinateSystem { get; private set; }

        public void ResetLocation()
        {
            CoordinateSystem = CoordinateSystem.Default;
        }

        public void LocalTranslate(Vector v)
        {
            CoordinateSystem = CoordinateSystem.LocallyTranslated(v);
        }

        public void GlobalTranslate(Vector v)
        {
            CoordinateSystem = CoordinateSystem.GloballyTranslated(v);
        }

        public void LocalRotate(float angle, Vector axis)
        {
            CoordinateSystem = CoordinateSystem.LocallyRotated(angle, axis);
        }

        public void GlobalRotate(float angle, Vector axis)
        {
            CoordinateSystem = CoordinateSystem.GloballyRotated(angle, axis);
        }

        public void LocalRotate(float pitch, float yaw, float roll)
        {
            CoordinateSystem = CoordinateSystem.LocallyRotated(pitch, yaw, roll);
        }

        public void GlobalRotate(float pitch, float yaw, float roll)
        {
            CoordinateSystem = CoordinateSystem.GloballyRotated(pitch, yaw, roll);
        }

        public override string ToString()
        {
            return String.Format("#<Entity {0}>", Name);
        }

        private static long NextID;
    }
}
