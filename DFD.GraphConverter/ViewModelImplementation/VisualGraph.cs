using System.Numerics;
using DFD.DataStructures.Interfaces;
using DFD.ViewModel.Interfaces;
using ICollapsibleNodeData = DFD.DataStructures.Interfaces.ICollapsibleNodeData;

namespace DFD.GraphConverter.ViewModelImplementation;

public class VisualGraph : IVisualGraph
{
    public IGraph<ICollapsibleNodeData> LogicalGraph { get; set; }
    public Vector2 Size { get; set; }
    public IReadOnlyCollection<IVisualGraphNode> Nodes { get; set; }
    public IReadOnlyCollection<IVisualObject> Flows { get; set; }
    public IReadOnlyCollection<IVisualObject> ArrowHeads { get; set; }
    public IReadOnlyCollection<IVisualText> TextLabels { get; set; }
}