using System.Numerics;
using DFD.Model.Interfaces;

namespace DFD.ViewModel.Interfaces
{
    public interface IVisualGraph
    {
        IGraph<ICollapsableGraphNode> LogicalGraph { get; }
        Vector2 Size { get; }
        IReadOnlyCollection<IVisualGraphNode> Nodes { get; }
    }
}
