using DFD.Model.Interfaces;

namespace DFD.Model;

public class Diagram : IDiagram
{
    public ICollection<IGraphEntity> Entities { get; init; }

    public Diagram(ICollection<IGraphEntity> entities)
    {
        Entities = entities;
    }
}