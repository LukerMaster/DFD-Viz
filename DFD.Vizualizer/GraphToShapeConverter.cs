using DFD.GraphConverter.ViewModelImplementation;
using DFD.ViewModel.Interfaces;
using SFML.Graphics;
using SFML.System;

namespace DFD.Vizualizer;

public class GraphToShapeConverter
{
    public ICollection<Drawable> ConvertToDrawables(IVisualGraph graph)
    {
        ICollection<Drawable> drawables = new List<Drawable>();
        foreach (var node in graph.Nodes)
        {
            SFML.Graphics.ConvexShape shape = new ConvexShape((uint)node.DrawPoints.Count);

            for (int i = 0; i < node.DrawPoints.Count; i++)
            {
                shape.SetPoint((uint)i, new Vector2f(node.DrawPoints[i].X, node.DrawPoints[i].Y));
            }
            shape.FillColor = new Color(50, (byte)(node.DrawOrder * 50), (byte)(node.DrawOrder * 30), 42);
            drawables.Add(shape);
        }

        return drawables;
    }
}