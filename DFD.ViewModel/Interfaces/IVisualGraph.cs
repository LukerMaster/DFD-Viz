namespace DFD.ViewModel.Interfaces
{
    public interface IVisualGraph
    {
        IReadOnlyCollection<IVisualGraphNode> Nodes { get; }
    }
}
