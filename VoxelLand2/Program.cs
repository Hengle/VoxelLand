using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using SharpGL;
using SharpGL.Controls;

namespace VoxelLand
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.Run(new MainForm());
        }
    }
}
