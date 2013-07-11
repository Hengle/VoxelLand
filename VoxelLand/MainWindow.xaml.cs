using System.Windows;
using System.Diagnostics;
using System;
using System.Text;
using System.Runtime.InteropServices;

using SharpGL;
using SharpGL.Enumerations;

namespace VoxelLand
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void gl_Initialized(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
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
            gl.ShaderSource(vert, @"
                #version 330

                uniform mat4 modelViewMatrix;
                uniform mat4 projectionMatrix;

                in vec4 vertex;

                out VoxelData
                {
                    vec4 color;
                } v;

                void main()
                {
                    gl_Position = projectionMatrix * modelViewMatrix * vertex;
                    v.color = vec4(1.0, 1.0, 1.0, 1.0);
                }");
            gl.CompileShader(vert);

            var geo = gl.CreateShader(OpenGL.GL_GEOMETRY_SHADER);
            gl.ShaderSource(geo, @"
                #version 330

                layout (points) in;
                layout (triangle_strip, max_vertices=24) out;

                uniform mat4 modelViewMatrix;
                uniform mat4 projectionMatrix;

                uniform vec4 corners[8] =
                {
                    vec4( 0.5,  0.5,  0.5, 1.0), // front, top,    right
                    vec4(-0.5,  0.5,  0.5, 1.0), // front, top,    left
                    vec4( 0.5, -0.5,  0.5, 1.0), // front, bottom, right
                    vec4(-0.5, -0.5,  0.5, 1.0), // front, bottom, left
                    vec4( 0.5,  0.5, -0.5, 1.0), // back,  top,    right
                    vec4(-0.5,  0.5, -0.5, 1.0), // back,  top,    left
                    vec4( 0.5, -0.5, -0.5, 1.0), // back,  bottom, right
                    vec4(-0.5, -0.5, -0.5, 1.0), // back,  bottom, left
                };

                uniform int faces[24] =
                {
                    0, 1, 2, 3, // front
                    7, 6, 3, 2, // bottom
                    7, 5, 6, 4, // back
                    4, 0, 6, 2, // right
                    1, 0, 5, 4, // top
                    3, 1, 7, 5  // left
                };

                uniform vec3 normals[6] =
                {
                    vec3( 0.0,  0.0,  1.0),
                    vec3( 0.0, -1.0,  0.0),
                    vec3( 0.0,  0.0,  1.0),
                    vec3( 1.0,  0.0,  0.0),
                    vec3( 0.0,  1.0,  0.0),
                    vec3(-1.0,  0.0,  0.0),
                };

                in VoxelData
                {
                    vec4 color;
                } vs[];

                out FragmentData 
                { 
                    vec3 normal; 
                    vec4 color; 
                } frag; 
 
                void main(void) 
                { 
                    for (int f=0; f<6; f++) 
                    { 
                        for (int c=0; c<4; c++) 
                        { 
                            gl_Position = gl_in[0].gl_Position + (projectionMatrix * modelViewMatrix * corners[faces[f * 4 + c]]); 
                            frag.color  = vs[0].color; 
                            frag.normal = normals[f]; 
                            EmitVertex(); 
                        } 
                        EndPrimitive(); 
                    } 
                } 
                ");
            gl.CompileShader(geo);

            var frag = gl.CreateShader(OpenGL.GL_FRAGMENT_SHADER);
            gl.ShaderSource(frag, @"
                #version 330

                uniform float time;

                in FragmentData
                {
                    vec3 normal;
                    vec4 color;
                } frag;

                out vec4 color;

                void main()
                {
                	// color = vec4(0.5 + (0.5 * sin(time)), 0.5 + (0.5 * cos(time)), 0.5 - (0.5 * cos(time)), 1.0);
                	color = vec4(abs(frag.normal.x), abs(frag.normal.y), abs(frag.normal.z), 1);
                }");
            gl.CompileShader(frag);

            prog = gl.CreateProgram();

            gl.AttachShader(prog, vert);
            gl.AttachShader(prog, geo);
            gl.AttachShader(prog, frag);

            gl.BindAttribLocation(prog, 0, "vertex");

            gl.LinkProgram(prog);

            time = gl.GetUniformLocation(prog, "time");
            modelView = gl.GetUniformLocation(prog, "modelViewMatrix");
            projection = gl.GetUniformLocation(prog, "projectionMatrix");
        }

        private void gl_Draw(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            modelViewMatrix = Transform.Translate(0.0f, 0.0f, -5.0f);

            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            gl.UseProgram(prog);

            gl.Uniform1(time, sw.ElapsedMilliseconds / 1000.0f);
            gl.UniformMatrix4(modelView, 1, false, modelViewMatrix);
            gl.UniformMatrix4(projection, 1, false, projectionMatrix);

            gl.DrawArrays(OpenGL.GL_POINTS, 0, 5);

            gl.Flush();
        }

        private void gl_Resized(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            int w = (int)glControl.ActualWidth;
            int h = (int)glControl.ActualHeight;

            gl.Viewport(0, 0, w, h);

            projectionMatrix = Transform.Perspective((float)(Math.PI / 4), w/(float)h, 0.1f, 100.0f);
        }

        private OpenGL gl;
        private int time;
        private int modelView;
        private int projection;
        private uint prog;
        private Matrix projectionMatrix;
        private Matrix modelViewMatrix;
        private Stopwatch sw = Stopwatch.StartNew();
    }
}