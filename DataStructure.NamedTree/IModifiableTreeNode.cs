using DataStructure.NamedTree;

namespace DFD.ModelImplementations;

public interface IModifiableTreeNode<T> : ITreeNode<T>
{
    public ITreeNode<T> Parent { get; set; }
    public ICollection<ITreeNode<T>> Children { get; }
}