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
    public class ShaderManager
    {
        public static void Initialize(OpenGL gl)
        {
            ShaderManager.gl = gl;

            vertexShaders = new Dictionary<string, VertexShader>();
            geometryShaders = new Dictionary<string, GeometryShader>();
            fragmentShaders = new Dictionary<string, FragmentShader>();
        }
        
        public static VertexShader GetVertexShader(string name)
        {
            if (gl == null)
                throw new InvalidOperationException("ShaderManager not initialized");

            if (!vertexShaders.ContainsKey(name))
            {
                vertexShaders[name] = new VertexShader(gl, name, new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(String.Format("VoxelLand.Shaders.Vertex.{0}.glsl", name))).ReadToEnd());
            }

            return vertexShaders[name];
        }

        public static GeometryShader GetGeometryShader(string name)
        {
            if (gl == null)
                throw new InvalidOperationException("ShaderManager not initialized");

            if (!geometryShaders.ContainsKey(name))
                geometryShaders[name] = new GeometryShader(gl, name, new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(String.Format("VoxelLand.Shaders.Geometry.{0}.glsl", name))).ReadToEnd());

            return geometryShaders[name];
        }

        public static FragmentShader GetFragmentShader(string name)
        {
            if (gl == null)
                throw new InvalidOperationException("ShaderManager not initialized");

            if (!fragmentShaders.ContainsKey(name))
                fragmentShaders[name] = new FragmentShader(gl, name, new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(String.Format("VoxelLand.Shaders.Fragment.{0}.glsl", name))).ReadToEnd());

            return fragmentShaders[name];
        }

        private static Dictionary<string, VertexShader> vertexShaders;
        private static Dictionary<string, GeometryShader> geometryShaders;
        private static Dictionary<string, FragmentShader> fragmentShaders;

        private static OpenGL gl;
    }
}
