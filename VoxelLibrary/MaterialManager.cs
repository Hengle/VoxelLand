using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpGL;
using SharpGL.Enumerations;
using System.IO;
using System.Reflection;

namespace VoxelLand
{
    public class MaterialManager
    {
        public static Dictionary<string, Tuple<string, string, string>> Materials = new Dictionary<string, Tuple<string, string, string>>()
        {
            //                       |----------------------Shaders-----------------|
            // Name                   Vertex         Geometry        Fragment         
            { "Voxels", Tuple.Create("PassThrough", "PointToVoxel", "ColorFromNormal") }
        };

        public static void Initialize(OpenGL gl)
        {
            MaterialManager.gl = gl;

            materials = new Dictionary<string, Material>();
        }
        
        public static Material GetMaterial(string name)
        {
            if (gl == null)
                throw new InvalidOperationException("MaterialManager not initialized");

            if (!materials.ContainsKey(name))
            {
                if (! Materials.ContainsKey(name))
                    throw new KeyNotFoundException("no material with that name found");

                var spec = Materials[name];

                Material m = new Material(gl, name);
                if (spec.Item1 != null) m.AddShader(ShaderManager.GetVertexShader(spec.Item1));
                if (spec.Item2 != null) m.AddShader(ShaderManager.GetGeometryShader(spec.Item2));
                if (spec.Item3 != null) m.AddShader(ShaderManager.GetFragmentShader(spec.Item3));
                m.Link();

                materials[name] = m;
            }

            return materials[name];
        }

        private static Dictionary<string, Material> materials;

        private static OpenGL gl;
    }
}
