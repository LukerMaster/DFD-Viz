using DFD.Model.Interfaces;
using DFD.ViewModel.Interfaces;

namespace DFD.GraphvizConverter;

public class CollapsableGraphNode : ICollapsableGraphNode
{
    public IGraphNodeData Data { get; set; }
    public bool ChildrenCollapsed { get; set; }
}