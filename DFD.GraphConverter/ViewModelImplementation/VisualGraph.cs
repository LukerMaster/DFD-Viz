using System.Numerics;
using DFD.Model.Interfaces;
using DFD.ViewModel.Interfaces;

namespace DFD.GraphConverter.ViewModelImplementation;

public class VisualGraph : IVisualGraph
{
    public IGraph<IEditableGraphNode> LogicalGraph { get; set; }
    public Vector2 Size { get; set; }
    public IReadOnlyCollection<IVisualGraphNode> Nodes { get; set; }
    public IReadOnlyCollection<IVisualObject> Flows { get; set; }
    public IReadOnlyCollection<IVisualObject> ArrowHeads { get; set; }
    public IReadOnlyCollection<IVisualText> TextLabels { get; set; }
}