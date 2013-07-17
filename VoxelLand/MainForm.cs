using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using SharpGL;
using SharpGL.Enumerations;

namespace VoxelLand
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            camera = new PerspectiveCamera();
            camera.LocalTranslate(new Vector(0, 0, -3));

            // camera = new OrthographicCamera(2);
            // camera.LocalTranslate(new Vector(0, 0, -1));
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            mainLoopThread = new Thread(mainLoop) { IsBackground = true };
            mainLoopThread.Start();
        }

        private void mainLoop(object ignore)
        {
            while (true)
            {
                BeginInvoke((Action)Redraw);
                Thread.Sleep(1);
            }
        }

        private void Redraw()
        {
            gl.Viewport(viewport.Left, viewport.Top, viewport.Width, viewport.Height);

            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            gl.UseProgram(prog);

            gl.UniformMatrix4(modelView, 1, false, camera.ModelViewMatrix);
            gl.UniformMatrix4(projection, 1, false, camera.GetProjectionMatrix(viewport));

            gl.DrawArrays(OpenGL.GL_POINTS, 0, 5);

            gl.Flush();
        }

        private void glControl_OpenGLDraw(object sender, SharpGL.RenderEventArgs args) { }

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
        }

        private void glControl_SizeChanged(object sender, EventArgs e)
        {
            viewport = new Viewport(0, 0, glControl.Width, glControl.Height);
        }

        private OpenGL gl;
        private int modelView;
        private int projection;
        private uint prog;
        private Thread mainLoopThread;
        private Camera camera;
        private Viewport viewport;
    }
}
