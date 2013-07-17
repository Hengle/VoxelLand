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
            this.id = NextID++;
            this.parent = parent;
            this.coordinateSystem = coordinateSystem ?? CoordinateSystem.Default;
            this.Name = name ?? String.Format("Entity #{0}", id);
        }

        public string Name { get; set; }

        public Matrix ModelViewMatrix
        {
            get
            {
                if (parent == null)
                    return coordinateSystem.ModelViewMatrix;
                else
                    return coordinateSystem.ModelViewMatrix * parent.ModelViewMatrix;
            }
        }

        public void LocalTranslate(Vector v)
        {
            coordinateSystem = coordinateSystem.LocallyTranslated(v);
        }

        public void GlobalTranslate(Vector v)
        {
            coordinateSystem = coordinateSystem.GloballyTranslated(v);
        }

        public void LocalRotate(float angle, Vector axis)
        {
            coordinateSystem = coordinateSystem.LocallyRotated(angle, axis);
        }

        public void GlobalRotate(float angle, Vector axis)
        {
            coordinateSystem = coordinateSystem.GloballyRotated(angle, axis);
        }

        public override string ToString()
        {
            return String.Format("#<Entity {0}>", Name);
        }

        protected Entity parent;
        protected CoordinateSystem coordinateSystem;
        protected long id;

        private static long NextID;
    }
}
