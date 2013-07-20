using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VoxelLand
{
    public class PhysicalEntity : Entity
    {
        public PhysicalEntity(Mesh mesh, Material material, CoordinateSystem coordinateSystem=null, string name="")
            : base(coordinateSystem, name)
        {
            Mesh = mesh;
            Material = material;
        }

        public Mesh Mesh { get; private set; }
        public Material Material { get; private set; }
    }
}
