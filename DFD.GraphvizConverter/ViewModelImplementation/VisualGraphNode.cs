using System.Numerics;
using DFD.ViewModel.Interfaces;

namespace DFD.GraphvizConverter.ViewModelImplementation;

public class VisualGraphNode : IVisualGraphNode
{
    public ICollapsableGraphNode Node { get; set; }
    public DisplayType Symbol { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Size { get; set; }
}