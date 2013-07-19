using System;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;

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

            renderer = new Renderer();
            renderer.Initialize(handle, viewport);

            camera = new PerspectiveCamera();
            camera.LocalTranslate(new Vector(0, 0, 3));

            // camera = new OrthographicCamera(2);
            // camera.LocalTranslate(new Vector(0, 0, -1));
        }

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
            bool updated = false;

            Vector d = mouse.Read();
            if (d.Y != 0.0f) { updated = true; camera.LocalRotate(-d.Y / 200.0f, Vector.UnitX); }
            if (d.X != 0.0f) { updated = true; camera.LocalRotate( d.X / 200.0f, camera.CoordinateSystem.ToLocal(Vector.UnitY)); }

            TimeSpan t;
            t = keyboard.Read(Keys.W);     if (t.TotalSeconds > 0.0f) { updated = true; camera.LocalTranslate(Vector.UnitZ * (float)t.TotalSeconds * -3.0f); }
            t = keyboard.Read(Keys.S);     if (t.TotalSeconds > 0.0f) { updated = true; camera.LocalTranslate(Vector.UnitZ * (float)t.TotalSeconds *  3.0f); }
            t = keyboard.Read(Keys.A);     if (t.TotalSeconds > 0.0f) { updated = true; camera.LocalTranslate(Vector.UnitX * (float)t.TotalSeconds * -3.0f); }
            t = keyboard.Read(Keys.D);     if (t.TotalSeconds > 0.0f) { updated = true; camera.LocalTranslate(Vector.UnitX * (float)t.TotalSeconds *  3.0f); }
            t = keyboard.Read(Keys.Space); if (t.TotalSeconds > 0.0f) { updated = true; camera.GlobalTranslate(Vector.UnitY * (float)t.TotalSeconds *  3.0f); }
            t = keyboard.Read(Keys.Z);     if (t.TotalSeconds > 0.0f) { updated = true; camera.GlobalTranslate(Vector.UnitY * (float)t.TotalSeconds * -3.0f); }

            if (updated)
                Debug.WriteLine("Camera: {0}", camera.CoordinateSystem);
        }

        private void Paint()
        {
            renderer.Render(viewport, camera);
        }

        private Thread mainLoopThread;
        private ManualResetEvent running;

        private Mouse mouse;
        private Keyboard keyboard;
        private Camera camera;
        private Viewport viewport;
        private Renderer renderer;
    }
}
