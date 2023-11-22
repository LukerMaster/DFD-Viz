using System.Numerics;
using DFD.Vizualizer.Model;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace DFD.Vizualizer;

public class DiagramUI
{
    protected DiagramPresenter _presenter;
    protected RenderWindow _window;

    protected IDiagramModel _model;

    public DiagramUI(RenderWindow window, IDiagramModel model)
    {
        _model = model;
        _window = window;
        
        _presenter = new DiagramPresenter(_model, _window);
        _window.MouseMoved += (sender, args) =>
        {
            foreach (var node in _model.NodeGraph.Nodes)
            {
                var inWorldCoords = _window.MapPixelToCoords(new Vector2i(args.X, args.Y));
                if (HitboxChecker.IsPointInPolygon4(node.DrawPoints, new Vector2(inWorldCoords.X, inWorldCoords.Y)))
                {
                    Console.WriteLine("Inside:" + node.Node.Data.Name);
                }
            }
        };
    }

    public void Process()
    {
        _presenter.Display();
    }
}