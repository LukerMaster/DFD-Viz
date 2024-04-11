using DataStructure.NamedTree;
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


        var queue = new Queue<ITreeNode<IMultilevelGraphNode>>();
        queue.Enqueue(multilevelGraph.Root);
        while (queue.Count > 0)
        {
            var node = queue.Dequeue();

            if (node.Children.Count > 0)
            {
                (node.Data as MultilevelGraphNode).Collapsable = true;
            }

            foreach (var child in node.Children)
            {
                queue.Enqueue(child);
            }
        }


        return multilevelGraph;
    }
}