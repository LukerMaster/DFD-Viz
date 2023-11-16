using DataStructure.NamedTree;

namespace DFD.Model.Interfaces;

public interface IFlow<T>
{
    ITreeNode<T> Source { get; }
    ITreeNode<T> Target { get; }
    bool BiDirectional { get; }
    string? FlowName { get; }
}