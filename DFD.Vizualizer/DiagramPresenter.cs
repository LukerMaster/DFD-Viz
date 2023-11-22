﻿using System.Numerics;
using DFD.Vizualizer.Model;
using SFML.Graphics;
using SFML.System;

namespace DFD.Vizualizer;

public class DiagramPresenter
{
    protected IDiagramModel _model;
    protected RenderWindow _window;

    public IDiagramModel Model { get => _model; set => _model = value; }

    private GraphToShapeConverter shapeConverter = new GraphToShapeConverter();

    private WindowManipulationHandler _windowManipulationHandler;

    public DiagramPresenter(IDiagramModel model, RenderWindow window)
    {
        _model = model;
        _window = window;

        _windowManipulationHandler = new WindowManipulationHandler(_window);
        _windowManipulationHandler.CenterViewTo(_model.NodeGraph.Size / 2);

    }
    public void Display()
    {
        _window.SetView(_windowManipulationHandler.CurrentView);
        _window.Clear(new Color(10, 10, 30));
        _window.Draw(_model.BgSprite);
        
        foreach (var drawable in shapeConverter.ConvertToDrawables(_model.NodeGraph))
        {
            _window.Draw(drawable);
        }
        _window.Display();
    }
}