using System.Numerics;
using DFD.ViewModel.Interfaces;

namespace DFD.GraphvizConverter.ViewModelImplementation;

public class VisualGraphNode : IVisualGraphNode
{
    public ICollapsableGraphNode Node { get; set; }
    public PointConnectionType PointConnectionType { get; set; }
    public IList<Vector2> DrawPoints { get; set; }
    public Vector2 TextPosition { get; set; }
}