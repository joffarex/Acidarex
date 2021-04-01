﻿using System;
using System.Numerics;
using AcidarX.Core.Events;
using AcidarX.Core.Input;

namespace AcidarX.Core.Camera
{
    public class OrthographicCameraController
    {
        private readonly float _cameraSpeed;
        private float _aspectRatio;
        private Vector3 _cameraPosition;
        private float _zoomLevel;
        private readonly float _zoomSpeed;

        public OrthographicCameraController(float aspectRatio)
        {
            _aspectRatio = aspectRatio;
            _zoomLevel = 1.0f;
            _zoomSpeed = 0.25f;
            _cameraSpeed = 1.5f;
            Camera = new OrthographicCamera(-_aspectRatio * _zoomLevel, _aspectRatio * _zoomLevel, -_zoomLevel,
                _zoomLevel);
        }

        public OrthographicCamera Camera { get; }

        public void OnUpdate(double deltaTime)
        {
            if (KeyboardState.IsKeyPressed(AXKey.A))
            {
                _cameraPosition.X -= _cameraSpeed * (float) deltaTime;
            }

            if (KeyboardState.IsKeyPressed(AXKey.D))
            {
                _cameraPosition.X += _cameraSpeed * (float) deltaTime;
            }

            if (KeyboardState.IsKeyPressed(AXKey.W))
            {
                _cameraPosition.Y += _cameraSpeed * (float) deltaTime;
            }

            if (KeyboardState.IsKeyPressed(AXKey.S))
            {
                _cameraPosition.Y -= _cameraSpeed * (float) deltaTime;
            }

            Camera.Position = _cameraPosition;
        }

        public void OnEvent(Event e)
        {
            var eventDispatcher = new EventDispatcher(e);
            eventDispatcher.Dispatch<MouseScrollEvent>(OnMouseScroll);
            eventDispatcher.Dispatch<WindowResizeEvent>(OnWindowResize);
        }

        private bool OnMouseScroll(MouseScrollEvent e)
        {
            _zoomLevel -= e.Offset.Y * _zoomSpeed;
            _zoomLevel = Math.Max(_zoomLevel, 0.25f);
            Camera.SetProjection(-_aspectRatio * _zoomLevel, _aspectRatio * _zoomLevel, -_zoomLevel,
                _zoomLevel);

            return false;
        }

        private bool OnWindowResize(WindowResizeEvent e)
        {
            _aspectRatio = (float) e.Size.X / e.Size.Y;
            Camera.SetProjection(-_aspectRatio * _zoomLevel, _aspectRatio * _zoomLevel, -_zoomLevel,
                _zoomLevel);

            return false;
        }
    }
}