using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using Avalonia;
using Avalonia.Controls;
using DFD.AvaloniaEditor.Interfaces;
using DFD.AvaloniaEditor.Services;
using DFD.GraphConverter.Interfaces;
using DFD.GraphConverter;
using DFD.GraphConverter.ViewModelImplementation;
using DFD.Model.Interfaces;
using DFD.ModelImplementations;
using DFD.Parsing.Interfaces;
using DFD.Parsing;
using DFD.ViewModel.Interfaces;

namespace DFD.AvaloniaEditor.ViewModels;

internal class DiagramViewModel : ViewModelBase
{
    public DiagramViewModel(IVisualGraphProvider diagramProvider)
    {
        Provider = diagramProvider;
    }

    public IVisualGraphProvider Provider { get; }

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

    private IReadOnlyList<Vector2> GetBezierCurve(IReadOnlyList<Vector2> controlPoints, int numPoints = 40)
    {
        List<Vector2> curvePoints = new List<Vector2>();
        for (int i = 0; i < numPoints; i++)
        {
            float t = i / (float)(numPoints - 1);
            curvePoints.Add(DeCasteljau(t, controlPoints));
        }

        return curvePoints;
    }

    private IVisualObject ToBezierCurve(IVisualObject vo)
    {
        var curve = GetBezierCurve(vo.Points);
        VisualObject newVO = new VisualObject()
        {
            Points = curve,
            DrawOrder = vo.DrawOrder,
            DrawTechnique = vo.DrawTechnique,
            IsClosed = vo.IsClosed
        };

        return newVO;
    }

    private IList<Points> PointsFor(IEnumerable<IVisualObject> objects)
    {
        var listOfNodes = new List<Points>();
        foreach (var visualObject in objects)
        {
            var points = new Points();
            foreach (var pointAsVector in visualObject.Points)
            {
                var point = new Point(pointAsVector.X, pointAsVector.Y);
                points.Add(point);
            }
            listOfNodes.Add(points);
        }
        return listOfNodes;
    }


    internal class PropertiedVector2
    {
        public float X { get; set; }
        public float Y { get; set; }

        public PropertiedVector2(Vector2 vector)
        {
            X = vector.X;
            Y = vector.Y;
        }
    }



    public IList<Points> PointsForNodes => PointsFor(Provider.VisualGraph.Nodes.Select(n => n.VisualObject));
    public IList<Points> PointsForFlows => PointsFor(Provider.VisualGraph.Flows.Select(flow => ToBezierCurve(flow)));
    public IList<Points> PointsForArrowHeads => PointsFor(Provider.VisualGraph.ArrowHeads);
}