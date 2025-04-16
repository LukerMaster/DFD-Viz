using System.Numerics;
using DFD.DataStructures.Interfaces;
using DFD.ViewModel.Interfaces;

namespace DFD.GraphConverter.ViewModelImplementation;

public class VisualGraphNode : IVisualGraphNode
{
    public required ICollapsibleNodeData Node { get; set; }
    public IVisualObject VisualObject { get; set; }
    public Vector2 TextPosition { get; set; }
}