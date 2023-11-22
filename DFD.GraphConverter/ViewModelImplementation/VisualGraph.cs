using System.Numerics;
using DFD.Model.Interfaces;
using DFD.ViewModel.Interfaces;

namespace DFD.GraphConverter.ViewModelImplementation;

public class VisualGraph : IVisualGraph
{
    public IGraph<ICollapsableGraphNode> LogicalGraph { get; set; }
    public Vector2 Size { get; set; }
    public IReadOnlyCollection<IVisualGraphNode> Nodes { get; set; }
}