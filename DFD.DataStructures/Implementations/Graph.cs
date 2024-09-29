using DFD.DataStructures.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;

namespace DFD.DataStructures.Implementations;

public class Graph<T> : IGraph<T>
{
    private INode<T> _root;
    private List<IFlow<T>> _flows = new();

    public Graph(INode<T> root, List<IFlow<T>> flows)
    {
        _root = root;
        _flows = flows;
    }

    INodeRef<T> IGraphRef<T>.Root => _root;

    IReadOnlyCollection<IFlow<T>> IGraph<T>.Flows => _flows;
    
    INode<T> IGraph<T>.Root => _root;

    IReadOnlyCollection<IFlowRef<T>> IGraphRef<T>.Flows => _flows;
}