namespace DFD.DataStructures.Interfaces;

public interface IGraph<T> : IGraphRef<T>
{
    INode<T> Root { get; }
    IReadOnlyCollection<IFlow<T>> Flows { get; }
    
    IGraph<TNew> CopyGraphAs(Func<T, TNew> dataConversionFunc)
}