using System.Numerics;
using DFD.ViewModel.Interfaces;
using DFD.Vizualizer.Interfaces;
using SFML.Graphics;
using SFML.System;

namespace DFD.Vizualizer;

public class DiagramPresenter : IDiagramPresenter
{
    private readonly IVisualGraphProvider _graphProvider;
    protected RenderWindow _window;
    

    private GraphToShapeConverter shapeConverter = new GraphToShapeConverter();

    private readonly WindowViewManipulator _windowViewManipulator;

    private ICollection<Drawable> _drawables;
    private IVisualGraph _previousFrameGraph;

    public DiagramPresenter(IVisualGraphProvider graphProvider, RenderWindow window, WindowViewManipulator viewManipulator)
    {
        _graphProvider = graphProvider;
        _window = window;

        _windowViewManipulator = viewManipulator;
        _windowViewManipulator.ResetView();

        _drawables = shapeConverter.ConvertToDrawables(_graphProvider.VisualGraph);

    }
    public void Display()
    {
        if (_graphProvider.VisualGraph != _previousFrameGraph)
        {
            _drawables = shapeConverter.ConvertToDrawables(_graphProvider.VisualGraph);
            _previousFrameGraph = _graphProvider.VisualGraph;
        }

        _window.SetView(_windowViewManipulator.CurrentView);
        _window.Clear(new Color(10, 10, 30));
        
        foreach (var drawable in _drawables)
        {
            _window.Draw(drawable);
        }
        _window.Display();
    }
}