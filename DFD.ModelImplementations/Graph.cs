using DataStructure.NamedTree;
using DFD.Model.Interfaces;

namespace DFD.ModelImplementations;

public class Graph<T> : IGraph<T>
{ 
    public ITreeNode<T> Root { get; set; }
    public ICollection<IFlow<T>> Flows { get; set; }
    public Graph(ITreeNode<T> root, ICollection<IFlow<T>> flows)
    {
        Root = root;
        Flows = flows;
    }

    public Graph()
    {

    }

}