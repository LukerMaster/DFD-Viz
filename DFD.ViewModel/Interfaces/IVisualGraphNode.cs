using System.Numerics;

namespace DFD.ViewModel.Interfaces;


public interface IVisualGraphNode
{
    ICollapsableGraphNode Node { get; }
    IList<Vector2> DrawPoints { get; }
    int DrawOrder { get; }
}