namespace DFD.DataStructures.Interfaces;

public interface IGraph<T> : INodeRef<T>
{
    INode<T> Root { get; }
    IReadOnlyCollection<IFlow<T>> Flows { get; }
    void AddNode(INode<T> parent);
}