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

        // Top level node cannot be collapsed - Graph would just vanish.
        (multilevelGraph.Root.Data as MultilevelGraphNode).Collapsible = false;


        var queue = new Queue<ITreeNode<IMultilevelGraphNode>>();

        foreach (var rootChild in multilevelGraph.Root.Children) 
            queue.Enqueue(rootChild);

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();

            if (node.Children.Count > 0)
            {
                (node.Data as MultilevelGraphNode).Collapsible = true;
            }

            foreach (var child in node.Children)
            {
                queue.Enqueue(child);
            }
        }


        return multilevelGraph;
    }
}