using DFD.Model.Interfaces;

namespace DFD.Model;

public class Graph : IGraph
{
    public IGraphEntity Root { get; init; }
    public ICollection<IFlow> Flows { get; }

    public Graph(IGraphEntity root, ICollection<IFlow> flows)
    {
        Root = root;
        Flows = flows;
    }
}