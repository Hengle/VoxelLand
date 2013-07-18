using System;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;

namespace VoxelLand
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            // FormBorderStyle = FormBorderStyle.None;
            // WindowState = FormWindowState.Maximized;
        }

        private void glControl_OpenGLDraw(object sender, SharpGL.RenderEventArgs args)
        {
        }

        private void glControl_OpenGLInitialized(object sender, EventArgs e)
        {
        }

        protected override void OnActivated(EventArgs e)
        {
            Debug.WriteLine("Activated");

            // foreground = true;
            // Capture = true;
            // Cursor.Hide();

            if (game != null)
                game.Resume();

            base.OnActivated(e);
        }

        protected override void OnDeactivate(EventArgs e)
        {
            if (game != null)
                game.Pause();

            // foreground = false;
            // Capture = false;
            // Cursor.Show();

            base.OnDeactivate(e);
        }

        // protected override void OnMouseMove(MouseEventArgs e)
        // {
        //     int cx = Left + Width / 2;
        //     int cy = Top + Height / 2;

        //     if (Cursor.Position.X != cx || Cursor.Position.Y != cy)
        //     {
        //         game.OnMouseMove(Cursor.Position.X - cx, cy - Cursor.Position.Y);
        //         Cursor.Position = new System.Drawing.Point(cx, cy);
        //     }
        // }

        // protected override void OnMouseCaptureChanged(EventArgs e)
        // {
        //     base.OnMouseCaptureChanged(e);

        //     BeginInvoke((Action)(() =>
        //         {
        //             if (foreground && !Capture)
        //                 Capture = true;
        //         }));
        // }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            game = new Game();
            game.Initialize(this.glControl.OpenGL, a => BeginInvoke(a));
            game.OnViewportChanged(new Viewport(0, 0, glControl.Width, glControl.Height));
            game.Start();
        }


        private void glControl_SizeChanged(object sender, EventArgs e)
        {
            if (game == null) return;
            game.OnViewportChanged(new Viewport(0, 0, glControl.Width, glControl.Height));
        }

        private bool foreground;
        private Game game;
    }
}