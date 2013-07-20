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

        public void Initialize(OpenGL gl)
        {
            this.gl = gl;

            gl.Enable(OpenGL.GL_DEPTH_TEST);
            gl.DepthFunc(DepthFunction.LessThanOrEqual);
            gl.ClearDepth(1.0);

            gl.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);

            gl.Hint(HintTarget.PerspectiveCorrection, HintMode.Nicest);
            gl.Hint(HintTarget.LineSmooth,            HintMode.Nicest);
            gl.Hint(HintTarget.PointSmooth,           HintMode.Nicest);
            gl.Hint(HintTarget.PolygonSmooth,         HintMode.Nicest);

            gl.Enable(OpenGL.GL_SMOOTH);
            gl.Enable(OpenGL.GL_LINE_SMOOTH);
            gl.Enable(OpenGL.GL_POINT_SMOOTH);
            gl.Enable(OpenGL.GL_MULTISAMPLE);

            gl.MinSampleShading(4.0f);
        }

        public void Render(Viewport viewport, Camera camera, Scene scene)
        {
            gl.Viewport(viewport.Left, viewport.Top, viewport.Width, viewport.Height);
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            foreach (var entities in scene.OfType<PhysicalEntity>().GroupBy(e => e.Material))
            {
                gl.UseProgram(entities.Key.ID);
                entities.Key.SetUniform("projectionMatrix", camera.GetProjectionMatrix(viewport));

                foreach (var entity in entities)
                {
                    gl.BindVertexArray(entity.Mesh.ID);
                    entity.Material.SetUniform("modelViewMatrix", camera.CoordinateSystem.ViewMatrix * entity.CoordinateSystem.ModelMatrix);
                    gl.DrawArrays((uint)entity.Mesh.Type, 0, entity.Mesh.Length);
                }
            }

            gl.Flush();
            gl.Blit(IntPtr.Zero);
        }

        private OpenGL gl;
    }
}