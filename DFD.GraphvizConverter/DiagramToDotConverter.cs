using System.Collections;
using DataStructure.NamedTree;
using DFD.Model.Interfaces;
using DFD.ModelImplementations;
using DFD.ViewModel.Interfaces;

namespace DFD.GraphvizConverter
{
    public class DiagramToDotConverter
    {
        private readonly string BiDirectionalAttribute = "[dir=both]";
        private string RepresentNode(ITreeNode<ICollapsableGraphNode> node, string code, bool useDisplayNames = false)
        {
            if (node.Children.Count > 0 && !node.Data.ChildrenCollapsed)
            {
                code += $"subgraph {node.FullNodeName.Replace('.', '_')} \n" +
                        $"{{ label=\"{node.Data.Data.Name}\" \n " +
                        $"cluster=True \n";
                foreach (var child in node.Children)
                {
                    code = RepresentNode(child, code);
                }
                code += "} \n";
            }
            else
            {
                var formatting = String.Empty;
                switch (node.Data.Data.Type)
                {
                    case NodeType.Process:
                        formatting = $"[label=\"{node.Data.Data.Name}\", shape=cds]";
                        break;
                    case NodeType.Storage:
                        formatting = $"[label=\"{node.Data.Data.Name}\",shape=cylinder]";
                        break;
                    case NodeType.InputOutput:
                        formatting = $"[label=\"{node.Data.Data.Name}\",style=rounded, shape=box]";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                code += $"{node.FullNodeName.Replace('.', '_')} {formatting}; \n";
            }
            return code;
        }

        private void CheckIfSubtreeHasCollapsedChildren(ITreeNode<ICollapsableGraphNode> node,
            ICollection<string> currentList)
        {
            if (node.Data.ChildrenCollapsed)
            {
                currentList.Add(node.FullNodeName);
            }
            else
            {
                foreach (var childNode in node.Children)
                {
                    CheckIfSubtreeHasCollapsedChildren(childNode, currentList);
                }
            }
        }

        private ICollection<string> GetAllNodeNamesWithCollapsedChildren(ITreeNode<ICollapsableGraphNode> node)
        {
            ICollection<string> collapsedList = new List<string>();
            CheckIfSubtreeHasCollapsedChildren(node, collapsedList);
            return collapsedList;
        }

        public string ToDot(IGraph<ICollapsableGraphNode> graph)
        {
            ICollection<string> collapsedNodesList = GetAllNodeNamesWithCollapsedChildren(graph.Root);

            var flowsInVisualGraph = CreateRedirectedFlowsIfNodesAreCollapsed(graph.Flows, collapsedNodesList);

            string code = "digraph { ";
            code = RepresentNode(graph.Root, code);
            foreach (var flow in flowsInVisualGraph)
            {
                var attribute = flow.BiDirectional ? BiDirectionalAttribute : String.Empty;

                code += $"{flow.SourceNodeName.Replace('.', '_')} -> {flow.TargetNodeName.Replace('.', '_')} [label=\"{flow.FlowName}\"] {attribute}; \n";
            }

            code += " } \n";
            return code;
        }

        private static ICollection<INodeFlow> CreateRedirectedFlowsIfNodesAreCollapsed(IReadOnlyCollection<INodeFlow> logicalFlows, ICollection<string> collapsedNodesList)
        {
            ICollection<INodeFlow> flowsInVisualGraph = new List<INodeFlow>();

            foreach (var logicalGraphFlow in logicalFlows)
            {
                var newFlow = new NodeFlow()
                {
                    BiDirectional = logicalGraphFlow.BiDirectional,
                    FlowName = logicalGraphFlow.FlowName,
                    SourceNodeName = logicalGraphFlow.SourceNodeName,
                    TargetNodeName = logicalGraphFlow.TargetNodeName
                };

                foreach (var nodeName in collapsedNodesList)
                {
                    if (newFlow.SourceNodeName.StartsWith(nodeName))
                        newFlow.SourceNodeName = nodeName;
                    if (newFlow.TargetNodeName.StartsWith(nodeName))
                        newFlow.TargetNodeName = nodeName;
                }

                flowsInVisualGraph.Add(newFlow);
            }

            return flowsInVisualGraph;
        }
    }
}
