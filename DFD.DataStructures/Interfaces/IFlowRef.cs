namespace DFD.DataStructures.Interfaces;

public interface IFlowRef<T>
{
    INodeRef<T> Source { get; }
    INodeRef<T> Target { get; }
    string Name { get; }
    bool IsBidirectional { get; }
}