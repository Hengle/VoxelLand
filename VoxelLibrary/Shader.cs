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
    public abstract class Shader
    {
        public Shader(OpenGL gl, uint shaderType, string name, string source)
        {
            Name = name;
            Source = source;

            ID = gl.CreateShader(shaderType);
            gl.ShaderSource(ID, Source);
            gl.CompileShader(ID);
        }

        public uint ID { get; private set; }

        public string Name { get; private set; }

        public string Source { get; private set; }

        public override string ToString()
        {
            return String.Format("#<shader {0}>", Name);
        }
    }

    public class VertexShader : Shader
    {
        public VertexShader(OpenGL gl, string name, string source)
            : base(gl, OpenGL.GL_VERTEX_SHADER, name, source)
        {
        }
    }

    public class GeometryShader : Shader
    {
        public GeometryShader(OpenGL gl, string name, string source)
            : base(gl, OpenGL.GL_GEOMETRY_SHADER, name, source)
        {
        }
    }

    public class FragmentShader : Shader
    {
        public FragmentShader(OpenGL gl, string name, string source)
            : base(gl, OpenGL.GL_FRAGMENT_SHADER, name, source)
        {
        }
    }
}
