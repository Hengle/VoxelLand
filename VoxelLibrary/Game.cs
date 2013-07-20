using System;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using SharpGL;

namespace VoxelLand
{
    public class Game
    {
        public Game()
        {
            mouse = new Mouse();
            keyboard = new Keyboard();
            running = new ManualResetEvent(false);
        }

        public void Initialize(IntPtr handle, Viewport viewport)
        {
            this.viewport = viewport;

            gl = new OpenGL();
            gl.Create(RenderContextType.NativeWindow, viewport.Width, viewport.Height, 32, handle);
            gl.MakeCurrent();

            ShaderManager.Initialize(gl);
            MaterialManager.Initialize(gl);

            renderer = new Renderer();
            renderer.Initialize(gl);

            camera = new PerspectiveCamera();
            camera.LocalTranslate(new Vector(0, 0, 3));

            var mesh = new Mesh(gl, MeshType.Points);
            mesh.AddBuffer(
                new Buffer<Point>(gl,
                    new Point(-2, -2, 0),
                    new Point(-1, -1, 0),
                    new Point( 0,  0, 0),
                    new Point( 1, -1, 0),
                    new Point( 2, -2, 0)));

            scene = new Scene();
            scene.Add(camera);

            for (int j = -10; j < 10; j++)
            for (int i = -10; i < 10; i++)
            {
                var e = new PhysicalEntity(mesh, MaterialManager.GetMaterial("Voxels"));
                e.GlobalTranslate(Vector.UnitZ * -5 * i);
                e.GlobalTranslate(Vector.UnitX * -5 * j);
                scene.Add(e);
            }

            gl.MakeNothingCurrent();
        }

        #region Pause/Resume
        public void Start()
        {
            running.Set();

            if (mainLoopThread == null)
            {
                mainLoopThread = new Thread(MainLoop) { IsBackground = true };
                mainLoopThread.Start();
            }
        }

        public void Pause()
        {
            running.Reset();
        }

        public void Resume()
        {
            running.Set();
        }
        #endregion

        #region External Events
        public void OnMouseMove(float dx, float dy)
        {
            mouse.OnMouseMove(dx, dy);
        }

        public void OnKeyUp(Keys key)
        {
            keyboard.OnKeyUp(key);
        }

        public void OnKeyDown(Keys key)
        {
            keyboard.OnKeyDown(key);
        }

        public void OnViewportChanged(Viewport viewport)
        {
            this.viewport = viewport;
        }
        #endregion

        private void MainLoop()
        {
            while (true)
            {
                running.WaitOne();
                ProcessInput();
                Paint();
                Thread.Sleep(1);
            }
        }

        private void ProcessInput()
        {
            Vector forward = camera.CoordinateSystem.ToGlobal(Vector.UnitZ);
            forward = new Vector(-forward.X, 0.0f, -forward.Z);

            Vector d = mouse.Read();
            if (d.Y != 0.0f) camera.LocalRotate( d.Y / 200.0f, Vector.UnitX);
            if (d.X != 0.0f) camera.LocalRotate(-d.X / 200.0f, camera.CoordinateSystem.ToLocal(Vector.UnitY));

            TimeSpan t;
            t = keyboard.Read(Keys.W);     if (t.TotalSeconds > 0.0f) camera.GlobalTranslate(forward * (float)t.TotalSeconds *  3.0f);
            t = keyboard.Read(Keys.S);     if (t.TotalSeconds > 0.0f) camera.GlobalTranslate(forward * (float)t.TotalSeconds * -3.0f);
            t = keyboard.Read(Keys.D);     if (t.TotalSeconds > 0.0f) camera.LocalTranslate(Vector.UnitX * (float)t.TotalSeconds *  3.0f);
            t = keyboard.Read(Keys.A);     if (t.TotalSeconds > 0.0f) camera.LocalTranslate(Vector.UnitX * (float)t.TotalSeconds * -3.0f);
            t = keyboard.Read(Keys.Space); if (t.TotalSeconds > 0.0f) camera.GlobalTranslate(Vector.UnitY * (float)t.TotalSeconds *  3.0f);
            t = keyboard.Read(Keys.Z);     if (t.TotalSeconds > 0.0f) camera.GlobalTranslate(Vector.UnitY * (float)t.TotalSeconds * -3.0f);
        }

        private void Paint()
        {
            gl.MakeCurrent();
            renderer.Render(viewport, camera, scene);
        }

        private Thread mainLoopThread;
        private ManualResetEvent running;
        private OpenGL gl;

        private Mouse mouse;
        private Keyboard keyboard;
        private Camera camera;
        private Viewport viewport;
        private Renderer renderer;
        private Scene scene;
    }
}
