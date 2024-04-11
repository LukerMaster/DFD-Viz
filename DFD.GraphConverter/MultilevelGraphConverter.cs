using DFD.GraphConverter.Interfaces;
using DFD.Model.Interfaces;
using DFD.ViewModel.Interfaces;

namespace DFD.GraphConverter;

public class MultilevelGraphConverter : IMultilevelGraphConverter
{
    public IGraph<IMultilevelGraphNode> ToMultilevelGraph(IGraph<IGraphNodeData> graph)
    {
        IGraph<IMultilevelGraphNode> multilevelGraph =
            graph.CopyGraphAs<IMultilevelGraphNode>(data => new MultilevelGraphNode()
            {
                Data = data,
                Collapsed = false
            });

        (multilevelGraph.Root.Data as MultilevelGraphNode).LockedFromCollapsing = true;

        return multilevelGraph;
    }
}