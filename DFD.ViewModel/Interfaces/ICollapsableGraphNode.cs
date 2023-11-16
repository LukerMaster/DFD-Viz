using DFD.Model.Interfaces;

namespace DFD.ViewModel.Interfaces;

public interface ICollapsableGraphNode
{
    IGraphNodeData Data { get; }
    bool ChildrenCollapsed { get; }
}