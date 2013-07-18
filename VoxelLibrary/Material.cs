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
    public class Material
    {
        public Material(OpenGL gl, string name)
        {
            this.gl = gl;

            uniforms = new Dictionary<string, int>();
            linked = false;

            Name = name;
            ID = gl.CreateProgram();
        }

        public string Name { get; private set; }

        public uint ID { get; private set; }

        public void AddShader(Shader shader)
        {
            if (linked) throw new InvalidOperationException("already linked");
            gl.AttachShader(ID, shader.ID);
        }

        public void Link()
        {
            if (linked) throw new InvalidOperationException("already linked");
            gl.LinkProgram(ID);
            linked = true;
        }

        public void SetUniform(string name, float f)
        {
            if (! linked) throw new InvalidOperationException("not yet linked");

            if (! uniforms.ContainsKey(name))
                uniforms[name] = gl.GetUniformLocation(ID, name);

            gl.Uniform1(uniforms[name], f);
        }

        public void SetUniform(string name, Matrix m)
        {
            if (! linked) throw new InvalidOperationException("not yet linked");

            if (! uniforms.ContainsKey(name))
                uniforms[name] = gl.GetUniformLocation(ID, name);

            gl.UniformMatrix4(uniforms[name], 1, false, m);
        }

        private OpenGL gl;
        private Dictionary<string, int> uniforms;
        private bool linked;
    }
}
