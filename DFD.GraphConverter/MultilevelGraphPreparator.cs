
using DFD.DataStructures.Interfaces;
using DFD.GraphConverter.Interfaces;

namespace DFD.GraphConverter;

public class MultilevelGraphPreparator : IMultilevelGraphPreparator
{
    public void TweakCollapsability(IGraph<ICollapsibleNodeData> graph)
    {
        // Top level node (root) shouldn't be collapsible.
        graph.Root.Data.Collapsible = false;
        // Don't show the root node on the graph.
        graph.Root.Data.IsHiddenAsParent = true;

        var queue = new Queue<INodeRef<ICollapsibleNodeData>>();

        foreach (var rootChild in graph.Root.Children) 
            queue.Enqueue(rootChild);

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();

            if (node.Children.Count > 0)
            {
                node.Data.Collapsible = true;
            }

            foreach (var child in node.Children)
            {
                queue.Enqueue(child);
            }
        }
    }
}