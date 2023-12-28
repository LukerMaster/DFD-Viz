using DFD.GraphConverter.Interfaces;
using DFD.Model.Interfaces;
using DFD.ViewModel.Interfaces;

namespace DFD.GraphConverter;

public class MultilevelGraphConverter : IMultilevelGraphConverter
{
    public IGraph<IEditableGraphNode> ToMultilevelGraph(IGraph<IGraphNodeData> graph)
    {
        IGraph<IEditableGraphNode> multilevelGraph =
            graph.CopyGraphAs<IEditableGraphNode>(data => new EditableGraphNode()
            {
                Data = data,
                ChildrenCollapsed = false
            });

        (multilevelGraph.Root.Data as EditableGraphNode).CanBeCollapsed = false;

        return multilevelGraph;
    }
}