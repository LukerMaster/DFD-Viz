using System.Drawing;
using System.Numerics;

namespace DFD.ViewModel.Interfaces;

public enum PointConnectionType
{
    Straight,
    Bezier
}


public interface IVisualGraphNode
{
    ICollapsableGraphNode Node { get; }
    PointConnectionType PointConnectionType { get; }
    IList<Vector2> DrawPoints { get; }
    Vector2 TextPosition { get; }
}