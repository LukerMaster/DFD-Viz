using DataStructure.NamedTree;

namespace DFD.Model.Interfaces;

public interface IFlow<T>
{
    ITreeNode<T> Source { get; set; }
    ITreeNode<T> Target { get; set; }
    bool BiDirectional { get; set; }
    string? FlowName { get; set; }
}