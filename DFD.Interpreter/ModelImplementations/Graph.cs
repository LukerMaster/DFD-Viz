using DataStructure.NamedTree;
using DFD.Model.Interfaces;

namespace DFD.Model;

public class Graph<T> : IGraph<T>
{
    public ITreeNode<T> Root { get; init; }
    public ICollection<IFlow<T>> Flows { get; }

    public Graph(ITreeNode<T> root, ICollection<IFlow<T>> flows)
    {
        Root = root;
        Flows = flows;
    }
}