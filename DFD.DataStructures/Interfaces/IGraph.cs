namespace DFD.DataStructures.Interfaces;

public interface IGraph<T> : IGraphRef<T>
{
    new INode<T> Root { get; }
    new IReadOnlyCollection<IFlow<T>> Flows { get; }
}