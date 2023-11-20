using DataStructure.NamedTree;
using DFD.Model.Interfaces;

namespace DFD.ModelImplementations;

public class Graph<T> : IGraph<T>
{
    public ITreeNode<T> Root { get; set; }
    public ICollection<INodeFlow<T>> Flows { get; set; }

    public Graph(ITreeNode<T> root, ICollection<INodeFlow<T>> flows)
    {
        Root = root;
        Flows = flows;
    }
}