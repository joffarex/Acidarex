﻿using System;
using AcidarX.Core.Events;
using AcidarX.Core.Renderer;
using Microsoft.Extensions.Logging;

namespace AcidarX.Core.Layers
{
    public class LayerFactory
    {
        private readonly GraphicsFactory _graphicsFactory;
        private readonly AXRenderer _renderer;

        public LayerFactory(GraphicsFactory graphicsFactory, AXRenderer renderer)
        {
            _graphicsFactory = graphicsFactory;
            _renderer = renderer;
        }

        public T CreateLayer<T>() where T : Layer =>
            (T) Activator.CreateInstance(typeof(T), _graphicsFactory, _renderer);
    }

    public abstract class Layer : IDisposable
    {
        private static readonly ILogger<Layer> Logger = AXLogger.CreateLogger<Layer>();

        public Layer(string debugName, GraphicsFactory graphicsFactory, AXRenderer renderer) : this(debugName)
        {
            GraphicsFactory = graphicsFactory;
            Renderer = renderer;
        }

        public Layer(string debugName) => DebugName = debugName;

        protected GraphicsFactory GraphicsFactory { get; }
        protected AXRenderer Renderer { get; }


        public string DebugName { get; }
        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            Logger.Assert(!IsDisposed, $"{this} is already disposed");

            IsDisposed = true;
            Dispose(true);
            // prevent the destructor from being called
            GC.SuppressFinalize(this);
            // make sure the garbage collector does not eat our object before it is properly disposed
            GC.KeepAlive(this);
        }

        public abstract void OnAttach();
        public abstract void OnLoad();
        public abstract void OnDetach();

        public virtual void OnUpdate(double deltaTime)
        {
        }

        public virtual void OnRender(double deltaTime)
        {
        }

        public virtual void OnImGuiRender()
        {
        }

        public virtual void OnEvent(Event e)
        {
        }

        public abstract void Dispose(bool manual);
    }
}