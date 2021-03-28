﻿using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace AcidarX.Core.Renderer.OpenGL
{
    public class OpenGLGraphicsContext : GraphicsContext
    {
        private readonly IWindow _window;

        public OpenGLGraphicsContext(IWindow window) => _window = window;

        public static GL Gl { get; private set; }


        public override void Init()
        {
            Gl = GL.GetApi(_window);
        }

        public override void Clear()
        {
            Gl.Clear((uint) ClearBufferMask.ColorBufferBit);
        }

        public override void ClearColor()
        {
            Gl.ClearColor(24 / 255.0f, 24 / 255.0f, 24 / 255.0f, 1.0f);
        }
    }
}