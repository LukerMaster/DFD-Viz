using DataStructure.NamedTree;
using DFD.Model.Interfaces;
using DFD.ViewModel.Interfaces;

namespace DFD.GraphvizConverter
{
    public class DiagramToDotConverter
    {
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

        public string ToDot(IGraph<ICollapsableGraphNode> graph)
        {
            string code = "digraph { ";
            code = RepresentNode(graph.Root, code);
            foreach (var flow in graph.Flows)
            {
                //code += $"{flow.SourceNodeName.Replace('.', '_')} -> {flow.TargetNodeName.Replace('.', '_')} [label=\"{flow.FlowName}\"]; \n";
            }

            code += " } \n";
            return code;
        }
    }
}
