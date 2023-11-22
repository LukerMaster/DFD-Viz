using DataStructure.NamedTree;
using DFD.Model.Interfaces;
using DFD.ViewModel.Interfaces;

namespace DFD.GraphvizConverter
{
    public class DiagramToDotConverter
    {
        private string RepresentNode(ITreeNode<ICollapsableGraphNode> node, string code, bool useDisplayNames = false)
        {
            foreach (var child in node.Children)
            {
                // If child node has children, draw them as subgraphs
                if (child.Children.Count > 0 && !node.Data.ChildrenCollapsed)
                {
                    code += $"subgraph {child.FullNodeName.Replace('.', '_')} \n" +
                            $"{{ label=\"{child.Data.Data.Name}\" \n " +
                            $"cluster=True \n";
                    code = RepresentNode(child, code);
                    code += "} \n";
                }
                // If child node is a leaf, draw it as node
                else
                {
                    var formatting = String.Empty;
                    switch (child.Data.Data.Type)
                    {
                        case NodeType.Process:
                            formatting = $"[label=\"{child.Data.Data.Name}\", shape=cds]";
                            break;
                        case NodeType.Storage:
                            formatting = $"[label=\"{child.Data.Data.Name}\",shape=cylinder]";
                            break;
                        case NodeType.InputOutput:
                            formatting = $"[label=\"{child.Data.Data.Name}\",style=rounded, shape=box]";
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    

                    code += $"{child.FullNodeName.Replace('.', '_')} {formatting}; \n";
                }
                
            }
            
            return code;
        }

        public string ToDot(IGraph<ICollapsableGraphNode> graph)
        {
            string code = "digraph { " +
                          "graph [dpi=400]";
            code = RepresentNode(graph.Root, code);
            foreach (var flow in graph.Flows)
            {
                code += $"{flow.Source.FullNodeName.Replace('.', '_')} -> {flow.Target.FullNodeName.Replace('.', '_')} [label=\"{flow.FlowName}\"]; \n";
            }

            code += " } \n";
            return code;
        }
    }
}
