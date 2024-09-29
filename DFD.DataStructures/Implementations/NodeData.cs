using DFD.DataStructures.Interfaces;

namespace DFD.DataStructures.Implementations;

public class NodeData : ICollapsibleNodeData
{
    public required string DisplayedName { get; set; }
    public NodeType Type { get; set; }
    public bool Collapsed { get; set; }
    public bool Collapsible { get; set; }
    public bool IsHiddenAsParent { get; set; }
}