using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharpGL;
using System.IO;
using SharpGL.Enumerations;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Diagnostics;
using System.Threading;

namespace VoxelLand
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            mainLoopThread = new Thread(mainLoop) { IsBackground = true };
            sw = Stopwatch.StartNew();
            frameTimes = new List<long>();
            n = 0;
        }

        private void mainLoop(object ignore)
        {
            while (true)
            {
                BeginInvoke((Action)(() => 
                    {
                        glControl_OpenGLDraw(null, null);
                    }));
            }
        }

        private void glControl_OpenGLDraw(object sender, SharpGL.RenderEventArgs args)
        {
            modelViewMatrix = Transform.Translate(0.0f, 0.0f, -5.0f);

            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            gl.UseProgram(prog);

            gl.UniformMatrix4(modelView, 1, false, modelViewMatrix);
            gl.UniformMatrix4(projection, 1, false, projectionMatrix);

            gl.DrawArrays(OpenGL.GL_POINTS, 0, 5);

            if (frameTimes.Count > 5 && (n++ % 200) == 0)
            {
                long elapsed = frameTimes[frameTimes.Count-1] - frameTimes[0];
                float fps = (frameTimes.Count * Stopwatch.Frequency) / (float)elapsed;
                Debug.WriteLine(String.Format("FPS: {0:F2}", fps));
            }

            gl.Flush();

            frameTimes.Add(sw.ElapsedTicks);
            if (frameTimes.Count > 100)
                frameTimes.RemoveAt(0);
        }

        private void glControl_OpenGLInitialized(object sender, EventArgs e)
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
            this.glControl.FrameRate = 60;

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

            modelView = gl.GetUniformLocation(prog, "modelViewMatrix");
            projection = gl.GetUniformLocation(prog, "projectionMatrix");

            mainLoopThread.Start();
        }

        private void glControl_SizeChanged(object sender, EventArgs e)
        {
            int w = (int)glControl.Width;
            int h = (int)glControl.Height;

            gl.Viewport(0, 0, w, h);

            projectionMatrix = Transform.Perspective((float)(Math.PI / 4), w/(float)h, 0.1f, 100.0f);
        }

        private OpenGL gl;
        private int modelView;
        private int projection;
        private uint prog;
        private Matrix projectionMatrix;
        private Matrix modelViewMatrix;
        private Stopwatch sw;
        private List<long> frameTimes;
        private int n;
        private Thread mainLoopThread;
    }
}
