using DFD.GraphConverter.Interfaces;
using DFD.Model.Interfaces;
using DFD.ViewModel.Interfaces;

namespace DFD.GraphConverter;

public class MultilevelGraphConverter : IMultilevelGraphConverter
{
    public IGraph<ICollapsableGraphNode> ToMultilevelGraph(IGraph<IGraphNodeData> graph)
    {
        IGraph<ICollapsableGraphNode> multilevelGraph =
            graph.CopyGraphAs<ICollapsableGraphNode>(data => new CollapsableGraphNode()
            {
                Data = data,
                ChildrenCollapsed = false
            });

        (multilevelGraph.Root.Data as CollapsableGraphNode).CanBeCollapsed = false;

        return multilevelGraph;
    }
}