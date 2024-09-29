using System.Numerics;

namespace DFD.ViewModel.Interfaces
{
    public interface IVisualGraph
    {
        Vector2 Size { get; }
        IReadOnlyCollection<IVisualGraphNode> Nodes { get; }
        IReadOnlyCollection<IVisualObject> Flows { get; }
        IReadOnlyCollection<IVisualObject> ArrowHeads { get; }
        IReadOnlyCollection<IVisualText> TextLabels { get; }
    }
}
