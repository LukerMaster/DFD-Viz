using DFD.Model.Interfaces;

namespace DFD.ViewModel.Interfaces;

public interface IEditableGraphNode
{
    IGraphNodeData Data { get; }
    bool ChildrenCollapsed { get; set; }
    bool IsHiddenAsParent { get; set; }
}