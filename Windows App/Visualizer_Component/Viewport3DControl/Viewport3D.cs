using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

namespace Viewport3DControl
{
    public partial class Viewport3D : UserControl
    {
        private GLControl gl;

        // Shaders (single program, position + color)
        private int shaderProgram = -1;
        private int uMvpLoc = -1;

        // Triangle buffers
        private int vaoTri = -1, vboTri = -1;

        // Grid buffers
        private int vaoGrid = -1, vboGrid = -1, gridVertexCount = 0;

        // World axes at origin
        private int vaoAxes = -1, vboAxes = -1, axesVertexCount = 0;

        // Camera
        private float yaw = 35f * (float)Math.PI / 180f;
        private float pitch = 20f * (float)Math.PI / 180f;
        private float distance = 4.0f;
        private System.Numerics.Vector3 target = System.Numerics.Vector3.Zero;

        // Interaction
        private Point lastMouse;
        private bool leftDown, rightDown;

        // Render loop
        private readonly Timer renderTimer;

        // Triangle data (System.Numerics)
        private System.Numerics.Vector3[] tri =
        {
            new System.Numerics.Vector3(-0.5f, -0.5f,  0.00f),
            new System.Numerics.Vector3( 0.5f, -0.5f,  0.25f), // slight +Z
            new System.Numerics.Vector3( 0.0f,  0.6f, -0.20f), // slight -Z
        };

        // Grid settings
        public float GridHalfSize { get; set; } = 5f;     // ±5 units
        public float GridSpacing { get; set; } = 0.5f;   // 0.5 unit spacing

        public Viewport3D()
        {
            DoubleBuffered = true;

            gl = new GLControl(
                new GraphicsMode(32, 24, 0, 0),
                3, 3,
                GraphicsContextFlags.Default)
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(120, 120, 120)
            };
            Controls.Add(gl);

            gl.Load += Gl_Load;
            gl.Paint += Gl_Paint;
            gl.Resize += Gl_Resize;

            gl.MouseDown += Gl_MouseDown;
            gl.MouseUp += Gl_MouseUp;
            gl.MouseMove += Gl_MouseMove;
            gl.MouseWheel += Gl_MouseWheel;

            renderTimer = new Timer { Interval = 16 };
            renderTimer.Tick += (s, e) => gl.Invalidate();
            renderTimer.Start();
        }

        #region Public API
        // Pose (degrees for convenience)
        private float rollDeg = 0f;   // about X
        private float pitchDeg = 0f;  // about Y
        private float yawDeg = 0f;    // about Z (optional, keep if you need)
        private float heave = 0f;     // +Y up (marine "heave")

        /// <summary>
        /// Set roll (X), pitch (Y), yaw (Z) in degrees, and heave (Y translation) in scene units.
        /// Call this whenever your real-time data updates.
        /// </summary>
        public void SetPoseRPYHeave(float rollDegrees, float pitchDegrees, float yawDegrees, float heaveUnits)
        {
            rollDeg = rollDegrees;
            pitchDeg = pitchDegrees;
            yawDeg = yawDegrees;
            heave = heaveUnits;
        }


        public void SetTriangle(System.Numerics.Vector3 a, System.Numerics.Vector3 b, System.Numerics.Vector3 c)
        {
            tri[0] = a; tri[1] = b; tri[2] = c;
            UpdateTriangleBuffer();
        }

        public void FrameTo(System.Numerics.Vector3 newTarget, float newDistance)
        {
            target = newTarget;
            distance = Math.Max(0.1f, newDistance);
        }

        #endregion

        #region GL lifecycle

        private void Gl_Load(object sender, EventArgs e)
        {
            if (!gl.Context.IsCurrent) gl.MakeCurrent();

            GL.ClearColor(0.10f, 0.12f, 0.14f, 1f);
            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.LineWidth(1f);

            shaderProgram = CreateColorShader();
            uMvpLoc = GL.GetUniformLocation(shaderProgram, "uMVP");

            CreateTriangleBuffers();
            CreateGridBuffers();
            CreateAxesBuffers();
        }

        private void Gl_Resize(object sender, EventArgs e)
        {
            if (!gl.Context.IsCurrent) gl.MakeCurrent();
            GL.Viewport(0, 0, Math.Max(1, gl.ClientSize.Width), Math.Max(1, gl.ClientSize.Height));
        }

        private void Gl_Paint(object sender, PaintEventArgs e)
        {
            if (!gl.Context.IsCurrent) gl.MakeCurrent();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Build matrices
            var proj = MakePerspective(45f, Math.Max(1, gl.ClientSize.Width), Math.Max(1, gl.ClientSize.Height), 0.05f, 1000f);
            var view = MakeViewMatrix();
            var model = BuildModelMatrix();

            // Keep your existing convention: earlier you used (model * view * proj)
            var vpNum = System.Numerics.Matrix4x4.Multiply(view, proj);          // VP
            var mvpNum = System.Numerics.Matrix4x4.Multiply(model, vpNum);        // M * VP

            var vpOtk = ToOpenTK(vpNum);
            var mvpOtk = ToOpenTK(mvpNum);

            GL.UseProgram(shaderProgram);

            // --- 1) GRID & WORLD AXES with VP (no model) ---
            GL.UniformMatrix4(uMvpLoc, false, ref vpOtk);
            DrawGrid();
            DrawAxes();

            // --- 2) TRIANGLE with full MVP (includes your pose) ---
            GL.UniformMatrix4(uMvpLoc, false, ref mvpOtk);
            DrawTriangle();

            // 3) Axis gizmo (overlay) — leave as-is
            DrawAxisGizmoOverlay(view);

            gl.SwapBuffers();
        }

        #endregion

        #region Buffers (Triangle / Grid / Axes)

        private void CreateTriangleBuffers()
        {
            vaoTri = GL.GenVertexArray();
            vboTri = GL.GenBuffer();

            GL.BindVertexArray(vaoTri);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboTri);
            // 3 vertices * (pos3 + col3)
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(3 * 6 * sizeof(float)), IntPtr.Zero, BufferUsageHint.DynamicDraw);

            // position (location=0)
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            // color (location=1)
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));

            UpdateTriangleBuffer();

            GL.BindVertexArray(0);
        }

        private void UpdateTriangleBuffer()
        {
            if (vboTri <= 0) return;
            if (!gl.Context.IsCurrent) gl.MakeCurrent();

            // Gray fill + dark outline effect (we’ll draw twice using same data)
            var c = new System.Numerics.Vector3(0.85f, 0.87f, 0.90f);
            float[] data =
            {
                tri[0].X, tri[0].Y, tri[0].Z,  c.X, c.Y, c.Z,
                tri[1].X, tri[1].Y, tri[1].Z,  c.X, c.Y, c.Z,
                tri[2].X, tri[2].Y, tri[2].Z,  c.X, c.Y, c.Z,
            };

            GL.BindBuffer(BufferTarget.ArrayBuffer, vboTri);
            GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, (IntPtr)(data.Length * sizeof(float)), data);
        }

        private void CreateGridBuffers()
        {
            // Build grid lines on XZ plane
            float half = GridHalfSize;
            float step = GridSpacing;
            int linesPerDir = (int)Math.Floor(half / step);
            int totalLines = (linesPerDir * 2 + 1) * 2; // X-parallel + Z-parallel
            // each line = 2 vertices; each vertex = pos3 + col3
            float majorAlpha = 0.9f;
            float minorAlpha = 0.5f; // not used (no blending), but keep intensity by color value
            var minorColor = new System.Numerics.Vector3(0.33f, 0.35f, 0.38f);
            var majorColor = new System.Numerics.Vector3(0.45f, 0.48f, 0.52f);

            var verts = new float[totalLines * 2 * 6];
            int idx = 0;

            // Lines parallel to X (vary Z)
            for (int i = -linesPerDir; i <= linesPerDir; i++)
            {
                float z = i * step;
                var col = (i == 0) ? majorColor : minorColor;
                // from (-half, 0, z) to (half, 0, z)
                verts[idx++] = -half; verts[idx++] = 0f; verts[idx++] = z; verts[idx++] = col.X; verts[idx++] = col.Y; verts[idx++] = col.Z;
                verts[idx++] = half; verts[idx++] = 0f; verts[idx++] = z; verts[idx++] = col.X; verts[idx++] = col.Y; verts[idx++] = col.Z;
            }

            // Lines parallel to Z (vary X)
            for (int i = -linesPerDir; i <= linesPerDir; i++)
            {
                float x = i * step;
                var col = (i == 0) ? majorColor : minorColor;
                // from (x, 0, -half) to (x, 0, half)
                verts[idx++] = x; verts[idx++] = 0f; verts[idx++] = -half; verts[idx++] = col.X; verts[idx++] = col.Y; verts[idx++] = col.Z;
                verts[idx++] = x; verts[idx++] = 0f; verts[idx++] = half; verts[idx++] = col.X; verts[idx++] = col.Y; verts[idx++] = col.Z;
            }

            gridVertexCount = totalLines * 2;

            vaoGrid = GL.GenVertexArray();
            vboGrid = GL.GenBuffer();

            GL.BindVertexArray(vaoGrid);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboGrid);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(verts.Length * sizeof(float)), verts, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0); // pos
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(1); // color
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));

            GL.BindVertexArray(0);
        }

        private void CreateAxesBuffers()
        {
            // World axes at origin (X red, Y green, Z blue)
            var r = new System.Numerics.Vector3(1, 0, 0);
            var g = new System.Numerics.Vector3(0, 1, 0);
            var b = new System.Numerics.Vector3(0, 0, 1);

            float len = 1.0f;
            float[] verts =
            {
                // X
                0,0,0,  r.X,r.Y,r.Z,   len,0,0,  r.X,r.Y,r.Z,
                // Y
                0,0,0,  g.X,g.Y,g.Z,   0,len,0,  g.X,g.Y,g.Z,
                // Z
                0,0,0,  b.X,b.Y,b.Z,   0,0,len,  b.X,b.Y,b.Z,
            };
            axesVertexCount = 6;

            vaoAxes = GL.GenVertexArray();
            vboAxes = GL.GenBuffer();

            GL.BindVertexArray(vaoAxes);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboAxes);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(verts.Length * sizeof(float)), verts, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));

            GL.BindVertexArray(0);
        }

        #endregion

        #region Draw helpers

        private void DrawGrid()
        {
            GL.BindVertexArray(vaoGrid);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.DrawArrays(PrimitiveType.Lines, 0, gridVertexCount);
            GL.BindVertexArray(0);
        }

        private void DrawAxes()
        {
            GL.BindVertexArray(vaoAxes);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.LineWidth(2f);
            GL.DrawArrays(PrimitiveType.Lines, 0, axesVertexCount);
            GL.LineWidth(1f);
            GL.BindVertexArray(0);
        }

        private void DrawTriangle()
        {
            // Filled pass (uses per-vertex gray)
            GL.BindVertexArray(vaoTri);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Disable(EnableCap.LineSmooth);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            // Wireframe overlay (dark outline)
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.LineWidth(2f);
            GL.Enable(EnableCap.LineSmooth);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            GL.LineWidth(1f);
            GL.BindVertexArray(0);
        }

        private void DrawAxisGizmoOverlay(System.Numerics.Matrix4x4 viewMain)
        {
            // Save viewport
            int[] vp = new int[4];
            GL.GetInteger(GetPName.Viewport, vp);

            int size = Math.Max(64, Math.Min(gl.ClientSize.Width, gl.ClientSize.Height) / 6); // adaptive size
            int margin = 10;
            int x = vp[2] - size - margin;
            int y = margin;

            // Set mini viewport
            GL.Viewport(x, y, size, size);

            // Extract rotation part of the view (upper-left 3x3) to make axes rotate with camera
            // Build a rotation-only view (no translation)
            var rotView = ExtractRotationOnly(viewMain);
            var proj = MakePerspective(35f, size, size, 0.01f, 10f);
            var model = System.Numerics.Matrix4x4.CreateScale(0.6f);
            var mvp = model * rotView * proj;
            var mvpOtk = ToOpenTK(mvp);

            // Render without depth so it’s always visible
            GL.Disable(EnableCap.DepthTest);
            GL.UseProgram(shaderProgram);
            GL.UniformMatrix4(uMvpLoc, false, ref mvpOtk);

            GL.BindVertexArray(vaoAxes);
            GL.LineWidth(3f);
            GL.DrawArrays(PrimitiveType.Lines, 0, axesVertexCount);
            GL.LineWidth(1f);
            GL.BindVertexArray(0);

            GL.Enable(EnableCap.DepthTest);

            // Restore viewport
            GL.Viewport(vp[0], vp[1], vp[2], vp[3]);
        }

        #endregion

        #region Camera & math

        private System.Numerics.Matrix4x4 BuildModelMatrix()
        {
            float r = rollDeg * (float)Math.PI / 180f;
            float p = pitchDeg * (float)Math.PI / 180f;
            float y = yawDeg * (float)Math.PI / 180f;

            var Rx = System.Numerics.Matrix4x4.CreateFromAxisAngle(new System.Numerics.Vector3(1, 0, 0), r);
            var Ry = System.Numerics.Matrix4x4.CreateFromAxisAngle(new System.Numerics.Vector3(0, 1, 0), p);
            var Rz = System.Numerics.Matrix4x4.CreateFromAxisAngle(new System.Numerics.Vector3(0, 0, 1), y);

            // yaw * pitch * roll (adjust if you want a different order)
            var R = System.Numerics.Matrix4x4.Multiply(System.Numerics.Matrix4x4.Multiply(Rz, Ry), Rx);

            var T = System.Numerics.Matrix4x4.CreateTranslation(0f, heave, 0f);

            // Rotate the local shape, then place it in world:
            return System.Numerics.Matrix4x4.Multiply(T, R);   // T * R
        }

        private System.Numerics.Matrix4x4 MakeViewMatrix()
        {
            var dir = SphericalToCartesian(distance, yaw, pitch);
            var eye = target + dir;
            return LookAt(eye, target, new System.Numerics.Vector3(0, 1, 0));
        }

        private static System.Numerics.Vector3 SphericalToCartesian(float r, float yawRad, float pitchRad)
        {
            float x = r * (float)Math.Cos(pitchRad) * (float)Math.Cos(yawRad);
            float y = r * (float)Math.Sin(pitchRad);
            float z = r * (float)Math.Cos(pitchRad) * (float)Math.Sin(yawRad);
            return new System.Numerics.Vector3(x, y, z);
        }

        private void Gl_MouseDown(object sender, MouseEventArgs e)
        {
            lastMouse = e.Location;
            if (e.Button == MouseButtons.Left) leftDown = true;
            if (e.Button == MouseButtons.Right) rightDown = true;
            gl.Focus();
        }

        private void Gl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) leftDown = false;
            if (e.Button == MouseButtons.Right) rightDown = false;
        }

        private void Gl_MouseMove(object sender, MouseEventArgs e)
        {
            int dx = e.X - lastMouse.X;
            int dy = e.Y - lastMouse.Y;
            lastMouse = e.Location;

            if (leftDown)
            {
                float rotSpeed = 0.01f;
                yaw += dx * rotSpeed;
                pitch -= dy * rotSpeed;
                pitch = Math.Max(-1.5f, Math.Min(1.5f, pitch));
            }
            else if (rightDown)
            {
                float panSpeed = distance * 0.0015f;

                var camPos = target + SphericalToCartesian(distance, yaw, pitch);
                var forward = Normalize(target - camPos);
                var right = Normalize(Cross(new System.Numerics.Vector3(0, 1, 0), forward));
                var up = Normalize(Cross(forward, right));

                target -= right * (dx * panSpeed);
                target += up * (dy * panSpeed);
            }
        }

        private void Gl_MouseWheel(object sender, MouseEventArgs e)
        {
            float zoomFactor = (float)Math.Pow(0.9, e.Delta / 120.0);
            distance = Math.Max(0.05f, distance * zoomFactor);
        }

        private static OpenTK.Matrix4 ToOpenTK(System.Numerics.Matrix4x4 m)
        {
            return new OpenTK.Matrix4(
                m.M11, m.M12, m.M13, m.M14,
                m.M21, m.M22, m.M23, m.M24,
                m.M31, m.M32, m.M33, m.M34,
                m.M41, m.M42, m.M43, m.M44
            );
        }

        private static System.Numerics.Matrix4x4 MakePerspective(float fovDeg, int w, int h, float zNear, float zFar)
        {
            float fov = fovDeg * (float)Math.PI / 180f;
            float aspect = w / (float)Math.Max(1, h);
            float f = 1f / (float)Math.Tan(fov / 2f);
            return new System.Numerics.Matrix4x4(
                f / aspect, 0, 0, 0,
                0, f, 0, 0,
                0, 0, (zFar + zNear) / (zNear - zFar), -1,
                0, 0, (2 * zFar * zNear) / (zNear - zFar), 0
            );
        }

        private static System.Numerics.Matrix4x4 LookAt(System.Numerics.Vector3 eye, System.Numerics.Vector3 center, System.Numerics.Vector3 up)
        {
            var f = Normalize(center - eye);
            var s = Normalize(Cross(f, up));
            var u = Cross(s, f);

            var rot = new System.Numerics.Matrix4x4(
                s.X, u.X, -f.X, 0,
                s.Y, u.Y, -f.Y, 0,
                s.Z, u.Z, -f.Z, 0,
                0, 0, 0, 1);

            var trans = System.Numerics.Matrix4x4.CreateTranslation(-eye.X, -eye.Y, -eye.Z);
            return trans * rot;
        }

        private static System.Numerics.Matrix4x4 ExtractRotationOnly(System.Numerics.Matrix4x4 view)
        {
            // Zero translation; keep rotation part of the view matrix
            var rot = view;
            rot.M41 = rot.M42 = rot.M43 = 0f;
            rot.M14 = rot.M24 = rot.M34 = 0f;
            rot.M44 = 1f;
            return rot;
        }

        private static System.Numerics.Vector3 Cross(System.Numerics.Vector3 a, System.Numerics.Vector3 b)
            => new System.Numerics.Vector3(
                a.Y * b.Z - a.Z * b.Y,
                a.Z * b.X - a.X * b.Z,
                a.X * b.Y - a.Y * b.X);

        private static System.Numerics.Vector3 Normalize(System.Numerics.Vector3 v)
        {
            float len = (float)Math.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z);
            return len > 1e-8f ? v / len : System.Numerics.Vector3.Zero;
        }

        #endregion

        #region Shader (position + color)

        private int CreateColorShader()
        {
            string vs = @"
#version 330
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aColor;
uniform mat4 uMVP;
out vec3 vColor;
void main() {
    vColor = aColor;
    gl_Position = uMVP * vec4(aPosition, 1.0);
}";
            string fs = @"
#version 330
in vec3 vColor;
out vec4 FragColor;
void main() {
    FragColor = vec4(vColor, 1.0);
}";

            int v = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(v, vs);
            GL.CompileShader(v);
            CheckShader(v, "Vertex");

            int f = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(f, fs);
            GL.CompileShader(f);
            CheckShader(f, "Fragment");

            int prog = GL.CreateProgram();
            GL.AttachShader(prog, v);
            GL.AttachShader(prog, f);
            GL.BindAttribLocation(prog, 0, "aPosition");
            GL.BindAttribLocation(prog, 1, "aColor");
            GL.LinkProgram(prog);

            int linked;
            GL.GetProgram(prog, GetProgramParameterName.LinkStatus, out linked);
            if (linked == 0)
            {
                string log = GL.GetProgramInfoLog(prog);
                throw new Exception("Program link error: " + log);
            }

            GL.DeleteShader(v);
            GL.DeleteShader(f);
            return prog;
        }

        private static void CheckShader(int s, string label)
        {
            int ok;
            GL.GetShader(s, ShaderParameter.CompileStatus, out ok);
            if (ok == 0)
            {
                string log = GL.GetShaderInfoLog(s);
                throw new Exception($"{label} shader compile error: {log}");
            }
        }

        #endregion
    }
}
