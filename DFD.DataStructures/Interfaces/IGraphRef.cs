namespace DFD.DataStructures.Interfaces;

public interface IGraphRef<T>
{
    INodeRef<T>? Root { get; }
    IReadOnlyCollection<IFlowRef<T>> Flows { get; }
}