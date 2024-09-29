namespace DFD.DataStructures.Interfaces;

public interface INode<T> : INodeRef<T>
{
    new T Data { get; set; }
    new string Name { get; set; }
    INodeRef<T> AddChild(T data, string name);
    protected new INodeRef<T>? Parent { get; set; }
    protected new ICollection<INodeRef<T>> Children { get; }
}