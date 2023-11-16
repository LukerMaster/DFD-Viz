using DFD.Model.Interfaces;

namespace DFD.ModelImplementations;

public class GraphNodeData : IGraphNodeData
{
    public string Name { get; set; }
    public NodeType Type { get; set; }
}

