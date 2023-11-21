using DFD.Model.Interfaces;
using DFD.ViewModel.Interfaces;

namespace DFD.GraphConverter;

public class CollapsableGraphNode : ICollapsableGraphNode
{
    public IGraphNodeData Data { get; set; }
    public bool ChildrenCollapsed { get; set; }
}