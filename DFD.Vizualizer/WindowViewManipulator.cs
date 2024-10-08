﻿using DFD.Vizualizer.Interfaces;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace DFD.Vizualizer;

public class WindowViewManipulator
{
    protected RenderWindow _window;
    protected IViewDataProvider _viewCenterProvider;

    protected View _currentView = new View();
    protected float _currentScale = 1.0f;

    protected bool _moving = false;
    protected Vector2f _previousMousePos = new(0, 0);

    public void ResetView()
    {
        _currentView.Center = new Vector2f(_viewCenterProvider.Center.X, _viewCenterProvider.Center.Y);
        if (_viewCenterProvider.Size.Y > _viewCenterProvider.Size.X)
            _currentScale =  _viewCenterProvider.Size.Y / _window.Size.Y;
        else
            _currentScale = _viewCenterProvider.Size.X / _window.Size.X;
        UpdateView();
    }

    private void UpdateView()
    {
        _currentView.Size = new Vector2f(_window.Size.X * _currentScale, _window.Size.Y * _currentScale);
    }
    public WindowViewManipulator(RenderWindow w, IViewDataProvider viewCenterProvider)
    {
        _window = w;
        _viewCenterProvider = viewCenterProvider;

        _currentView.Center = new Vector2f(0, 0);

        UpdateView();

        _window.Resized += (sender, args) => UpdateView();

        _window.MouseWheelScrolled += (sender, args) =>
        {
            _currentScale *= (float)Math.Pow(0.75f, args.Delta);
            UpdateView();
        };

        _window.MouseButtonPressed += (sender, args) =>
        {
            if (args.Button == Mouse.Button.Left)
                _moving = true;
        };

        _window.MouseButtonReleased += (sender, args) =>
        {
            if (args.Button == Mouse.Button.Left)
                _moving = false;
        };

        _window.MouseMoved += (sender, args) =>
        {
            if (_moving)
            {
                Vector2f delta = (new Vector2f(args.X, args.Y) - _previousMousePos) * _currentScale;
                Vector2f newPos = _currentView.Center - delta;
                _currentView.Center = new Vector2f(_currentView.Center.X - delta.X, _currentView.Center.Y - delta.Y);
            }
            _previousMousePos = new Vector2f(args.X, args.Y);
        };

        _window.KeyPressed += (sender, args) =>
        {
            if (args.Code == Keyboard.Key.Space)
            {
                ResetView();
            }
        };
    }

    public View CurrentView
    {
        get => _currentView;
    }
}