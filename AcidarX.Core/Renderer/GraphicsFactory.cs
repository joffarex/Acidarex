﻿using System;
using AcidarX.Core.Layers;
using AcidarX.Core.Renderer.OpenGL;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace AcidarX.Core.Renderer
{
    public class GraphicsFactory
    {
        public GraphicsFactory(GL gl) => Gl = gl;

        public GL Gl { get; }

        public IndexBuffer CreateIndexBuffer<T>(T[] indices)
            where T : unmanaged
        {
            return AXRenderer.API switch
            {
                API.None => null,
                API.OpenGL => new OpenGLIndexBuffer<T>(Gl, new ReadOnlySpan<T>(indices)),
                _ => throw new Exception("Not supported API")
            };
        }

        public VertexBuffer CreateVertexBuffer<T>(T[] vertices)
            where T : unmanaged
        {
            return AXRenderer.API switch
            {
                API.None => null,
                API.OpenGL => new OpenGLVertexBuffer<T>(Gl, new ReadOnlySpan<T>(vertices)),
                _ => throw new Exception("Not supported API")
            };
        }

        public VertexArray CreateVertexArray()
        {
            return AXRenderer.API switch
            {
                API.None => null,
                API.OpenGL => new OpenGLVertexArray(Gl),
                _ => throw new Exception("Not supported API")
            };
        }

        public Shader CreateShader(string vertexSource, string fragmentSource)
        {
            return AXRenderer.API switch
            {
                API.None => null,
                API.OpenGL => new OpenGLShader(Gl, vertexSource, fragmentSource),
                _ => throw new Exception("Not supported API")
            };
        }

        public Texture2D CreateTexture(string path)
        {
            return AXRenderer.API switch
            {
                API.None => null,
                API.OpenGL => new OpenGLTexture2D(Gl, path),
                _ => throw new Exception("Not supported API")
            };
        }

        public ImGuiLayer CreateImGuiLayer(IWindow window, IInputContext inputContext) => new(Gl, window, inputContext);
    }
}