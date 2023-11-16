using System.Numerics;
using DFD.ViewModel.Interfaces;

namespace DFD.GraphvizConverter;

public class VisualGraphNode : IVisualGraphNode
{
    public string DisplayedText { get; set; }
    public DisplayType Symbol { get; set; }
    public bool HasChildrenCollapsed { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Size { get; set; }
}