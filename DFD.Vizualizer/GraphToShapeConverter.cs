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


    private Drawable GetLineFrom(IReadOnlyList<Vector2> Points, Color color)
    {
        VertexArray va = new VertexArray(PrimitiveType.LineStrip, (uint)Points.Count);
        for (int i = 0; i < Points.Count; i++)
        {
            va[(uint)i] = new Vertex(new Vector2f(Points[i].X, Points[i].Y), color);
        }
        return va;
    }

    private Drawable GetShapeFrom(IReadOnlyList<Vector2> Points, Color color)
    {
        ConvexShape shape = new ConvexShape((uint)Points.Count);
        for (int i = 0; i < Points.Count; i++)
        {
            shape.SetPoint((uint)i, new Vector2f(Points[i].X, Points[i].Y));
            shape.FillColor = color;
        }
        return shape;
    }
    private ICollection<Drawable> GetDrawablesFrom(IVisualObject vo, Color outLineColor, Color fillColor)
    {
        ICollection<Drawable> drawables = new List<Drawable>();
        if (vo.IsClosed)
        {
            drawables.Add(GetShapeFrom(vo.Points, Color.White));

            var enclosedPoints = new List<Vector2>(vo.Points);
            enclosedPoints.Add(vo.Points[0]); // loop back to beginning
            drawables.Add(GetLineFrom(enclosedPoints, Color.Black));
        }
        else
        {
            drawables.Add(GetLineFrom(vo.Points, Color.Black));
        }
        return drawables;
    }

    private Drawable GetLabelFrom(IVisualText textDefinition, Font font, Color color)
    {
        // Original font size is making the text extremely low-res, so we artificially scale it up.
        float scaleFactor = 8;

        Text text = new Text(textDefinition.Text, font);
        text.Position = new Vector2f(textDefinition.Position.X, textDefinition.Position.Y);
        text.CharacterSize = (uint)(textDefinition.FontSize * scaleFactor);
        text.Scale = new Vector2f(1 / scaleFactor, 1 / scaleFactor);
        text.FillColor = color;
        text.Origin = new Vector2f(textDefinition.Origin.X * text.GetLocalBounds().Width, textDefinition.Origin.Y * text.GetLocalBounds().Height);
        return text;
    }

    public ICollection<Drawable> ConvertToDrawables(IVisualGraph graph)
    {
        List<Drawable> drawables = new List<Drawable>();
        foreach (var node in graph.Nodes)
        {
            drawables.AddRange(GetDrawablesFrom(node.VisualObject, Color.Black, Color.White));
        }

        foreach (var arrowHead in graph.ArrowHeads)
        {
            drawables.Add(GetShapeFrom(arrowHead.Points, Color.Black));
        }

        foreach (var flow in graph.Flows)
        {
            var curvePoints = GetBezierCurve(flow.Points);
            
            drawables.Add(GetLineFrom(curvePoints, Color.Black));
        }

        Font font = new Font(Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "times.ttf"));
        foreach (var text in graph.TextLabels)
        {
            drawables.Add(GetLabelFrom(text, font, Color.Black));
        }

        return drawables;
    }
}