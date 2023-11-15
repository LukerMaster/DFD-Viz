using DFD.Model.Interfaces;

namespace DFD.Model;

public class Graph : IGraph
{
    public ICollection<IGraphEntity> Entities { get; init; }
    public ICollection<IFlow> Flows { get; }

    public Graph(ICollection<IGraphEntity> entities, ICollection<IFlow> flows)
    {
        Entities = entities;
        Flows = flows;
    }
}