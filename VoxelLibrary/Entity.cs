using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VoxelLand
{
    public abstract class Entity
    {
        public Entity(Entity parent=null, CoordinateSystem coordinateSystem=null, string name="")
        {
            ID = NextID++;
            Parent = parent;
            CoordinateSystem = coordinateSystem ?? CoordinateSystem.Default;
            Name = name.Length > 0 ? name : String.Format("Entity #{0}", ID);
        }

        public long ID { get; private set; }

        public string Name { get; set; }

        public Entity Parent { get; private set; }

        public CoordinateSystem CoordinateSystem { get; private set; }

        public void ResetLocation()
        {
            CoordinateSystem = CoordinateSystem.Default;
        }

        public Matrix ModelViewMatrix
        {
            get
            {
                if (Parent == null)
                    return CoordinateSystem.ModelViewMatrix;
                else
                    return CoordinateSystem.ModelViewMatrix * Parent.ModelViewMatrix;
            }
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

        public override string ToString()
        {
            return String.Format("#<Entity {0}>", Name);
        }

        private static long NextID;
    }
}
