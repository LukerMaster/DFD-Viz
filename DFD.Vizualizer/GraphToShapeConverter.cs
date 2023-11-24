using DFD.GraphConverter.ViewModelImplementation;
using DFD.ViewModel.Interfaces;
using SFML.Graphics;
using SFML.System;
using System.Numerics;

namespace DFD.Vizualizer;

public class GraphToShapeConverter
{
    private Vector2 DeCasteljau(float t, IReadOnlyList<Vector2> controlPoints)
    {
        if (controlPoints.Count == 1)
        {
            return controlPoints[0];
        }

        List<Vector2> newPoints = new List<Vector2>();
        for (int i = 0; i < controlPoints.Count - 1; i++)
        {
            Vector2 newPoint = (1 - t) * controlPoints[i] + t * controlPoints[i + 1];
            newPoints.Add(newPoint);
        }

        return DeCasteljau(t, newPoints);
    }

    private IReadOnlyList<Vector2> GetBezierCurve(IReadOnlyList<Vector2> controlPoints, int numPoints = 100)
    {
        List<Vector2> curvePoints = new List<Vector2>();
        for (int i = 0; i < numPoints; i++)
        {
            float t = i / (float)(numPoints - 1);
            curvePoints.Add(DeCasteljau(t, controlPoints));
        }

        return curvePoints;
    }

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
            shape.FillColor = new Color(50, (byte)(node.DrawOrder * 50), (byte)(node.DrawOrder * 30), 82);
            drawables.Add(shape);
        }

        foreach (var arrowHead in graph.ArrowHeads)
        {
            ConvexShape shape = new ConvexShape((uint)arrowHead.Points.Count);
            for (int i = 0; i < arrowHead.Points.Count; i++)
            {
                shape.SetPoint((uint)i, new Vector2f(arrowHead.Points[i].X, arrowHead.Points[i].Y));
                shape.FillColor = new Color(30, 90, 90, 120);
            }
            drawables.Add(shape);
        }

        foreach (var flow in graph.Flows)
        {
            var curvePoints = GetBezierCurve(flow.Points);
            VertexArray array = new VertexArray(PrimitiveType.LineStrip, (uint)curvePoints.Count);
            for (int i = 0; i < curvePoints.Count; i++)
            {
                array[(uint)i] = new Vertex(new Vector2f(curvePoints[i].X, curvePoints[i].Y), new Color(20, 200, 20, 150));
            }
            drawables.Add(array);
        }

        return drawables;
    }
}