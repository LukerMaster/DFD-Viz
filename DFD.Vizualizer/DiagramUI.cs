using System.Numerics;
using DFD.GraphConverter;
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

    protected DiagramLoader loader = new DiagramLoader();
    public DiagramUI(RenderWindow window, IDiagramModel model)
    {
        _model = model;
        _window = window;
        
        _presenter = new DiagramPresenter(_model, _window);

        _window.MouseMoved += (sender, args) =>
        {
        };

        _window.MouseButtonPressed += (sender, args) =>
        {
            foreach (var node in _model.NodeGraph.Nodes.Reverse())
            {
                var inWorldCoords = _window.MapPixelToCoords(new Vector2i(args.X, args.Y));
                if (HitboxChecker.IsPointInPolygon4(node.DrawPoints, new Vector2(inWorldCoords.X, inWorldCoords.Y)))
                {
                    
                    node.Node.ChildrenCollapsed = !node.Node.ChildrenCollapsed;
                    Console.WriteLine($"{node.Node.Data.Name} : Collapesed: {node.Node.ChildrenCollapsed}");
                    loader.ReloadModelDataBasedOn(_model);
                    break;
                }
            }
        };
    }

    public void Process()
    {
        _presenter.Display();
    }
}