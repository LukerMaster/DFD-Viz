namespace DFD.DataStructures.Interfaces;

public interface ICollapsibleNodeData : INodeData
{
    bool Collapsed { get; set; }
    bool Collapsible { get; set; }
    bool IsHiddenAsParent { get; set; }
}