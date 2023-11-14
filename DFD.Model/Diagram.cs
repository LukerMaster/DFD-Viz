using DFD.Model.Interfaces;

namespace DFD.Model;

public class Diagram : IDiagram
{
    public ICollection<IGraphEntity> Entities { get; init; }
    public ICollection<IFlow> Flows { get; }

    public Diagram(ICollection<IGraphEntity> entities, ICollection<IFlow> flows)
    {
        Entities = entities;
        Flows = flows;
    }
}