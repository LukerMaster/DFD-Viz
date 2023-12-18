using System.Collections.Generic;
using System.Linq;
using Avalonia;
using DFD.ViewModel.Interfaces;

namespace DFD.AvaloniaEditor.ViewModels.AvaloniaGraph;

internal class AvaloniaVisualGraph : IAvaloniaVisualGraph
{
    public IReadOnlyCollection<AvaloniaVisualNode> Nodes { get; set; }
    public IReadOnlyCollection<AvaloniaVisualObject> Flows { get; set; }
    public IReadOnlyCollection<AvaloniaVisualObject> ArrowHeads { get; set; }
    public IReadOnlyCollection<AvaloniaCanvasText> TextLabels { get; set; }


    private Point DeCasteljau(float t, Points controlPoints)
    {
        if (controlPoints.Count == 1)
        {
            return controlPoints[0];
        }

        Points newPoints = new Points();
        for (int i = 0; i < controlPoints.Count - 1; i++)
        {
            Point newPoint = (1 - t) * controlPoints[i] + t * controlPoints[i + 1];
            newPoints.Add(newPoint);
        }

        return DeCasteljau(t, newPoints);
    }

    private Points GetBezierCurve(Points controlPoints, int numPoints = 40)
    {
        Points curvePoints = new Points();
        for (int i = 0; i < numPoints; i++)
        {
            float t = i / (float)(numPoints - 1);
            curvePoints.Add(DeCasteljau(t, controlPoints));
        }

        return curvePoints;
    }

    private AvaloniaVisualObject ToAvaloniaObject(IVisualObject obj)
    {
        var newVO = new AvaloniaVisualObject();
        foreach (var pointAsVector in obj.Points)
        {
            var point = new Point(pointAsVector.X, pointAsVector.Y);
            newVO.Points.Add(point);
        }
        return newVO;
    }

    public AvaloniaVisualGraph(IVisualGraph? visualGraph)
    {
        Flows = visualGraph.Flows.Select(flow =>
        {
            var avaloniaObject = ToAvaloniaObject(flow);
            avaloniaObject.Points = GetBezierCurve(avaloniaObject.Points);
            return avaloniaObject;
        }).ToList();
        ArrowHeads = visualGraph.ArrowHeads.Select(ToAvaloniaObject).ToList();

        TextLabels = visualGraph.TextLabels.Select(label => new AvaloniaCanvasText(label)).ToList();

        Nodes = visualGraph.Nodes.Select(node => new AvaloniaVisualNode()
        {
            Node = node.Node,
            VisualObject = ToAvaloniaObject(node.VisualObject)
        }).ToList();
    }
}