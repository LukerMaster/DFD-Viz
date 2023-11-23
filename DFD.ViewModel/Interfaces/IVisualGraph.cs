using System.Numerics;
using DFD.Model.Interfaces;

namespace DFD.ViewModel.Interfaces
{
    public interface IVisualGraph
    {
        Vector2 Size { get; }
        IReadOnlyCollection<IVisualGraphNode> Nodes { get; }
    }
}
