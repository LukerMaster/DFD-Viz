using DFD.Model.Interfaces;
using DFD.ViewModel.Interfaces;

namespace DFD.GraphConverter;

public class MultilevelGraphCreator
{
    public IGraph<ICollapsableGraphNode> CreateMultiLevelGraphOutOf(IGraph<IGraphNodeData> codeGraph)
    {
        IGraph<ICollapsableGraphNode> multilevelGraph =
            codeGraph.CopyGraphAs<ICollapsableGraphNode>(data => new CollapsableGraphNode()
            {
                Data = data,
                ChildrenCollapsed = false
            });
        return multilevelGraph;
    }
}