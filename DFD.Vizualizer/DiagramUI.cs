using System.Numerics;
using DFD.GraphConverter;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace DFD.Vizualizer;

public class DiagramUI
{
    protected DiagramPresenter _presenter;
    protected VisualGraphProvider _provider;
    protected RenderWindow _window;
    public DiagramUI(RenderWindow window,
        VisualGraphProvider provider,
        DiagramPresenter presenter)
    {
        _window = window;
        _provider = provider;
        _presenter = presenter;
        
        _window.MouseMoved += (sender, args) =>
        {
        };

        _window.MouseButtonPressed += (sender, args) =>
        {
            foreach (var node in _provider.VisualGraph.Nodes.Reverse())
            {
                var inWorldCoords = _window.MapPixelToCoords(new Vector2i(args.X, args.Y));
                if (HitboxChecker.IsPointInPolygon4(node.DrawPoints, new Vector2(inWorldCoords.X, inWorldCoords.Y)))
                {
                    node.Node.ChildrenCollapsed = !node.Node.ChildrenCollapsed;
                    Console.WriteLine($"{node.Node.Data.Name} : Collapesed: {node.Node.ChildrenCollapsed}");
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