namespace DFD.DataStructures.Interfaces
{
    public interface INodeRef<T>
    {
        T Data { get; }
        string Name { get; }
        INodeRef<T>? Parent { get; }
        IReadOnlyCollection<INodeRef<T>> Children { get; }
        string FullPath { get; }
        string HexHash { get; }
    }
}
