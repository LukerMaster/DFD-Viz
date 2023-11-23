using System.Numerics;
using SFML.Graphics;
using SFML.System;

namespace DFD.Vizualizer;

public class DiagramPresenter
{
    private readonly VisualGraphProvider _graphProvider;
    protected RenderWindow _window;
    

    private GraphToShapeConverter shapeConverter = new GraphToShapeConverter();

    private WindowManipulationHandler _windowManipulationHandler;

    public DiagramPresenter(VisualGraphProvider graphProvider, RenderWindow window)
    {
        _graphProvider = graphProvider;
        _window = window;

        _windowManipulationHandler = new WindowManipulationHandler(_window);
        _windowManipulationHandler.CenterViewTo(_graphProvider.VisualGraph.Size / 2);

    }
    public void Display()
    {
        _window.SetView(_windowManipulationHandler.CurrentView);
        _window.Clear(new Color(10, 10, 30));
        
        foreach (var drawable in shapeConverter.ConvertToDrawables(_graphProvider.VisualGraph))
        {
            _window.Draw(drawable);
        }
        _window.Display();
    }
}