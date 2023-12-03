using System.Numerics;
using DFD.GraphConverter;
using DFD.Vizualizer.Interfaces;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace DFD.Vizualizer;

public class ProgramUI : IProgramUI
{
    protected IDiagramPresenter _presenter;
    protected IVisualGraphProvider _provider;
    protected RenderWindow _window;

    private Vector2 _mousePressPosition;

    public ProgramUI(RenderWindow window,
        IVisualGraphProvider provider,
        IDiagramPresenter presenter)
    {
        _window = window;
        _provider = provider;
        _presenter = presenter;
        
        _window.MouseMoved += (sender, args) =>
        {
        };

        _window.MouseButtonReleased += (sender, args) =>
        {
            if (args.Button == Mouse.Button.Left && new Vector2(args.X, args.Y) == _mousePressPosition)
            {
                foreach (var node in _provider.VisualGraph.Nodes.Reverse())
                {
                    var inWorldCoords = _window.MapPixelToCoords(new Vector2i(args.X, args.Y));
                    if (HitboxChecker.IsPointInPolygon4(node.VisualObject.Points, new Vector2(inWorldCoords.X, inWorldCoords.Y)))
                    {
                        node.Node.ChildrenCollapsed = !node.Node.ChildrenCollapsed;
                        break;
                    }
                }
            }
        };

        _window.MouseButtonPressed += (sender, args) =>
        {
            if (args.Button == Mouse.Button.Left)
                _mousePressPosition = new Vector2(args.X, args.Y);
        };
    }

    public void Process()
    {
        _presenter.Display();
    }
}