namespace DFD.DataStructures.Interfaces;

public interface INode<T> : INodeRef<T>
{
    new T Data { get; set; }
    new string Name { get; set; }
}