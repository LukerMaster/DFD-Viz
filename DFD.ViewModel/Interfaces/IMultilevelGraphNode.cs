using DFD.Model.Interfaces;

namespace DFD.ViewModel.Interfaces;

public interface IMultilevelGraphNode
{
    IGraphNodeData Data { get; }
    bool ChildrenCollapsed { get; set; }
    bool IsHiddenAsParent { get; set; }
}