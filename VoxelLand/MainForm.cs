using System;
using System.Windows.Forms;

namespace VoxelLand
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            game = new Game();
            foreground = false;

            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);

            // FormBorderStyle = FormBorderStyle.None;
            // WindowState = FormWindowState.Maximized;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            game.Initialize(Handle, new Viewport(0, 0, Width, Height));
            game.Start();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            game.OnViewportChanged(new Viewport(0, 0, Width, Height));
            base.OnSizeChanged(e);
        }

        protected override void OnActivated(EventArgs e)
        {
            int cx = Left + Width / 2;
            int cy = Top + Height / 2;
            Cursor.Position = new System.Drawing.Point(cx, cy);

            foreground = true;
            Capture = true;
            Cursor.Hide();

            game.Resume();

            base.OnActivated(e);
        }

        protected override void OnDeactivate(EventArgs e)
        {
            game.Pause();

            foreground = false;
            Capture = false;
            Cursor.Show();

            base.OnDeactivate(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            int cx = Left + Width / 2;
            int cy = Top + Height / 2;

            if (Cursor.Position.X != cx || Cursor.Position.Y != cy)
            {
                game.OnMouseMove(Cursor.Position.X - cx, cy - Cursor.Position.Y);
                Cursor.Position = new System.Drawing.Point(cx, cy);
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            game.OnKeyUp(e.KeyCode);
            base.OnKeyUp(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Application.Exit();
                return;
            }

            game.OnKeyDown(e.KeyCode);
            base.OnKeyDown(e);
        }

        protected override void OnMouseCaptureChanged(EventArgs e)
        {
            base.OnMouseCaptureChanged(e);

            BeginInvoke((Action)(() =>
                {
                    if (foreground && !Capture)
                        Capture = true;
                }));
        }

        private bool foreground;
        private Game game;
    }
}