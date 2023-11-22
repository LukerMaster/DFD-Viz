using System.Numerics;
using DFD.ViewModel.Interfaces;

namespace DFD.GraphConverter.ViewModelImplementation;

public class VisualGraph : IVisualGraph
{
    public Vector2 Size { get; set; }
    public IReadOnlyCollection<IVisualGraphNode> Nodes { get; set; }
}