using System.Numerics;
using DFD.ViewModel.Interfaces;

namespace DFD.GraphConverter.ViewModelImplementation;

public class VisualGraphNode : IVisualGraphNode
{
    public ICollapsableGraphNode Node { get; set; }
    public IList<Vector2> DrawPoints { get; set; }
    public int DrawOrder { get; set; }
    public Vector2 TextPosition { get; set; }
}