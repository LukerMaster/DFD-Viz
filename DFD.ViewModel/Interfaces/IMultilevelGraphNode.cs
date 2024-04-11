using DFD.Model.Interfaces;

namespace DFD.ViewModel.Interfaces;

public interface IMultilevelGraphNode
{
    IGraphNodeData Data { get; }
    bool Collapsed { get; set; }
    bool Collapsable { get; }
    bool IsHiddenAsParent { get; set; }
}