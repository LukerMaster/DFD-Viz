using System.Numerics;
using DFD.ViewModel.Interfaces;

namespace DFD.GraphConverter.ViewModelImplementation;

public class VisualGraphNode : IVisualGraphNode
{
    public ICollapsableGraphNode Node { get; set; }
    public IVisualObject VisualObject { get; set; }
    public Vector2 TextPosition { get; set; }
}