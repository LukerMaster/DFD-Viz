using System.Numerics;
using SFML.Graphics;
using SFML.System;

namespace DFD.Vizualizer;

public class DiagramPresenter : IDiagramPresenter
{
    private readonly IVisualGraphProvider _graphProvider;
    protected RenderWindow _window;
    

    private GraphToShapeConverter shapeConverter = new GraphToShapeConverter();

    private WindowViewManipulator _windowViewManipulator;

    public DiagramPresenter(IVisualGraphProvider graphProvider, RenderWindow window, WindowViewManipulator viewManipulator)
    {
        _graphProvider = graphProvider;
        _window = window;

        _windowViewManipulator = viewManipulator;
        _windowViewManipulator.ResetView();

    }
    public void Display()
    {
        _window.SetView(_windowViewManipulator.CurrentView);
        _window.Clear(new Color(10, 10, 30));
        
        foreach (var drawable in shapeConverter.ConvertToDrawables(_graphProvider.VisualGraph))
        {
            _window.Draw(drawable);
        }
        _window.Display();
    }
}