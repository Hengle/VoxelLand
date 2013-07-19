using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpGL;
using SharpGL.Enumerations;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace VoxelLand
{
    public class Renderer
    {
        public Renderer()
        {
        }

        public void Initialize(IntPtr handle, Viewport viewport)
        {
            gl = new OpenGL();
            gl.Create(RenderContextType.NativeWindow, viewport.Width, viewport.Height, 32, handle);
            gl.MakeCurrent();

            ShaderManager.Initialize(gl);
            MaterialManager.Initialize(gl);

            gl.Enable(OpenGL.GL_DEPTH_TEST);
            gl.DepthFunc(DepthFunction.LessThanOrEqual);
            gl.ClearDepth(1.0);

            gl.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);

            gl.Hint(HintTarget.PerspectiveCorrection, HintMode.Nicest);
            gl.Hint(HintTarget.LineSmooth, HintMode.Nicest);
            gl.Hint(HintTarget.PointSmooth, HintMode.Nicest);
            gl.Hint(HintTarget.PolygonSmooth, HintMode.Nicest);

            gl.Enable(OpenGL.GL_SMOOTH);
            gl.Enable(OpenGL.GL_LINE_SMOOTH);
            gl.Enable(OpenGL.GL_POINT_SMOOTH);
            gl.Enable(OpenGL.GL_MULTISAMPLE);

            gl.MinSampleShading(4.0f);

            material = MaterialManager.GetMaterial("Voxels");

            mesh = new Mesh(gl);
            mesh.AddBuffer(
                new Buffer<Point>(gl,
                    new Point(-2, -2, 0),
                    new Point(-1, -1, 0),
                    new Point( 0,  0, 0),
                    new Point( 1,  1, 0),
                    new Point( 2,  2, 0)));

            gl.MakeNothingCurrent();
        }

        public void Render(Viewport viewport, Camera camera)
        {
            gl.MakeCurrent();

            gl.Viewport(viewport.Left, viewport.Top, viewport.Width, viewport.Height);
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            gl.BindVertexArray(mesh.ID);

            gl.UseProgram(material.ID);
            
            material.SetUniform("modelViewMatrix", camera.CoordinateSystem.ViewMatrix);
            material.SetUniform("projectionMatrix", camera.GetProjectionMatrix(viewport));

            gl.DrawArrays(OpenGL.GL_POINTS, 0, 5);

            gl.Flush();
            gl.Blit(IntPtr.Zero);
        }

        private OpenGL gl;
        private Material material;
        private Mesh mesh;
    }
}
