using System;
using System.Threading;

namespace VoxelLand
{
    public class Game
    {
        public Game()
        {
            running = new ManualResetEvent(false);
            mouseAccum = Vector.Zero;
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
            mouseAccum += new Vector(dx, dy, 0);
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
            if (mouseAccum.Y != 0.0f)
                camera.LocalRotate(-mouseAccum.Y / 200.0f, Vector.UnitX);

            if (mouseAccum.X != 0.0f)
                camera.LocalRotate(mouseAccum.X / 200.0f, camera.CoordinateSystem.ToLocal(Vector.UnitY));

            mouseAccum.X = mouseAccum.Y = 0;
        }

        private void Paint()
        {
            renderer.Render(viewport, camera);
        }

        private Vector mouseAccum;
        private Thread mainLoopThread;
        private Renderer renderer;
        private Camera camera;
        private Viewport viewport;
        private ManualResetEvent running;
    }
}
