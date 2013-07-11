using System.Windows;
using System.Diagnostics;
using System;
using System.Text;
using System.Runtime.InteropServices;

using SharpGL;
using SharpGL.Enumerations;
using SharpGL.SceneGraph.Shaders;
using System.Reflection;
using System.IO;
using System.Collections.Generic;

namespace VoxelLand
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            frameTimes = new List<long>();
            sw = Stopwatch.StartNew();
        }

        private void gl_Initialized(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            float[] voxels = new float[]
            {
                -2.0f, -2.0f, 0.0f, 1.0f,
                -1.0f, -1.0f, 0.0f, 1.0f,
                 0.0f,  0.0f, 0.0f, 1.0f,
                 1.0f,  1.0f, 0.0f, 1.0f,
                 2.0f,  2.0f, 0.0f, 1.0f,
            };

            IntPtr voxelsMemory = Marshal.AllocHGlobal((int)(sizeof(float) * voxels.Length));
            Marshal.Copy(voxels, 0, voxelsMemory, voxels.Length);

            gl = this.glControl.OpenGL;
            this.glControl.FrameRate = 60.0;

            gl.Enable(OpenGL.GL_DEPTH_TEST);
            gl.DepthFunc(DepthFunction.LessThanOrEqual);
            gl.ClearDepth(1.0);
            gl.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);

            uint[] vaos = new uint[1];
            gl.GenVertexArrays(1, vaos);
            gl.BindVertexArray(vaos[0]);

            uint[] vbos = new uint[1];
            gl.GenBuffers(1, vbos);
            gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, vbos[0]);
            gl.BufferData(OpenGL.GL_ARRAY_BUFFER, voxels.Length * sizeof(float), voxelsMemory, OpenGL.GL_STATIC_DRAW);

            gl.VertexAttribPointer(0, 4, OpenGL.GL_FLOAT, false, 0, IntPtr.Zero);
            gl.EnableVertexAttribArray(0);

            var vert = gl.CreateShader(OpenGL.GL_VERTEX_SHADER);
            gl.ShaderSource(vert, new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("VoxelLand.Shaders.Vertex.PassThrough.glsl")).ReadToEnd());
            gl.CompileShader(vert);

            var geo = gl.CreateShader(OpenGL.GL_GEOMETRY_SHADER);
            gl.ShaderSource(geo, new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("VoxelLand.Shaders.Geometry.PointToVoxel.glsl")).ReadToEnd());
            gl.CompileShader(geo);

            var frag = gl.CreateShader(OpenGL.GL_FRAGMENT_SHADER);
            gl.ShaderSource(frag, new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("VoxelLand.Shaders.Fragment.ColorFromNormal.glsl")).ReadToEnd());
            gl.CompileShader(frag);

            prog = gl.CreateProgram();
            gl.AttachShader(prog, vert);
            gl.AttachShader(prog, geo);
            gl.AttachShader(prog, frag);
            gl.BindAttribLocation(prog, 0, "vertex");
            gl.LinkProgram(prog);

            time = gl.GetUniformLocation(prog, "time");
            modelView = gl.GetUniformLocation(prog, "modelViewMatrix");
            projection = gl.GetUniformLocation(prog, "projectionMatrix");
        }

        private void gl_Draw(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            modelViewMatrix = Transform.Translate(0.0f, 0.0f, -5.0f);

            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            gl.UseProgram(prog);

            gl.UniformMatrix4(modelView, 1, false, modelViewMatrix);
            gl.UniformMatrix4(projection, 1, false, projectionMatrix);

            gl.DrawArrays(OpenGL.GL_POINTS, 0, 5);

            if (frameTimes.Count > 5)
            {
                long elapsed = frameTimes[frameTimes.Count-1] - frameTimes[0];
                if (elapsed % 20 == 0)
                {
                    float fps = (frameTimes.Count * Stopwatch.Frequency) / (float)elapsed;
                    Debug.WriteLine(String.Format("FPS: {0:F2}", fps));
                }
            }

            gl.Flush();

            frameTimes.Add(sw.ElapsedTicks);
            if (frameTimes.Count > 100)
                frameTimes.RemoveAt(0);
        }

        private void gl_Resized(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            int w = (int)glControl.ActualWidth;
            int h = (int)glControl.ActualHeight;

            gl.Viewport(0, 0, w, h);

            projectionMatrix = Transform.Perspective((float)(Math.PI / 4), w/(float)h, 0.1f, 100.0f);
        }

        private OpenGL gl;
        private int time;
        private int modelView;
        private int projection;
        private uint prog;
        private Matrix projectionMatrix;
        private Matrix modelViewMatrix;
        private Stopwatch sw;
        private List<long> frameTimes;
    }
}