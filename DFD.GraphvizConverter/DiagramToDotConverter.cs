using DFD.DataStructures.Implementations;
using DFD.DataStructures.Interfaces;
using DFD.GraphvizConverter.API;
using ICollapsibleNodeData = DFD.DataStructures.Interfaces.ICollapsibleNodeData;
using NodeType = DFD.DataStructures.Interfaces.NodeType;

namespace DFD.GraphvizConverter
{
    public class DiagramToDotConverter
    {
        
        private readonly string lineTerminator = "\n";
        private readonly string BiDirectionalAttribute = "[dir=both]";
        private string RepresentNode(INodeRef<ICollapsibleNodeData> node, string code, bool useDisplayNames = false)
        {
            if (node.Children.Count > 0 && !node.Data.Collapsed)
            {
                if (node.Data.IsHiddenAsParent)
                {
                    code += $"subgraph {node.HexHash} {lineTerminator}" +
                            $"{{ peripheries=0 {lineTerminator} " +
                            $"cluster=True {lineTerminator}";
                }
                else
                {
                    code += $"subgraph {node.HexHash} {lineTerminator}" +
                            $"{{ label=\"{node.Data.DisplayedName}\" {lineTerminator} " +
                            $"peripheries=1 {lineTerminator}" +
                            $"cluster=True {lineTerminator}";
                }

                foreach (var child in node.Children)
                {
                    code = RepresentNode(child, code);
                }
                code += $"}} {lineTerminator}";
            }
            else
            {
                var formatting = String.Empty;
                switch (node.Data.Type)
                {
                    case NodeType.Process:
                        formatting = $"[label=\"{node.Data.DisplayedName}\", shape=cds]";
                        break;
                    case NodeType.Storage:
                        formatting = $"[label=\"{node.Data.DisplayedName}\",shape=cylinder]";
                        break;
                    case NodeType.InputOutput:
                        formatting = $"[label=\"{node.Data.DisplayedName}\",style=rounded, shape=box]";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                code += $"{node.HexHash} {formatting}; {lineTerminator}";
            }
            return code;
        }

        public string ToDot(DataStructures.Interfaces.IGraph<ICollapsibleNodeData> graph)
        {
            var flowsInVisualGraph = CreateRedirectedFlowsIfNodesAreCollapsed(graph.Flows);

            string code = "digraph { ";
            code = RepresentNode(graph.Root, code);
            foreach (var flow in flowsInVisualGraph)
            {
                var attribute = flow.IsBidirectional ? BiDirectionalAttribute : String.Empty;

                code += $"{flow.Source.HexHash} -> {flow.Target.HexHash} [label=\"{flow.Name}\"] {attribute}; {lineTerminator}";
            }

            code += $" }} {lineTerminator}";
            return code;
        }

        private static ICollection<IFlowRef<ICollapsibleNodeData>> CreateRedirectedFlowsIfNodesAreCollapsed(IReadOnlyCollection<IFlow<ICollapsibleNodeData>> logicalFlows)
        {
            ICollection<IFlowRef<ICollapsibleNodeData>> flowsInVisualGraph = new List<IFlowRef<ICollapsibleNodeData>>();

            foreach (var logicalGraphFlow in logicalFlows)
            {
                var newFlow = new Flow<ICollapsibleNodeData>()
                {
                    IsBidirectional = logicalGraphFlow.IsBidirectional,
                    Name = logicalGraphFlow.Name,
                    Source = logicalGraphFlow.Source,
                    Target = logicalGraphFlow.Target
                };

                if (newFlow.Source.FindEarliestAncestorThatMatches(d => d.Collapsible && d.Collapsed) is var srcAncestor && srcAncestor is not null)
                    newFlow.Source = srcAncestor;
                if (newFlow.Target.FindEarliestAncestorThatMatches(d => d.Collapsible && d.Collapsed) is var tgtAncestor && tgtAncestor is not null)
                    newFlow.Target = tgtAncestor;

                flowsInVisualGraph.Add(newFlow);
            }

            return flowsInVisualGraph;
        }
    }
}
