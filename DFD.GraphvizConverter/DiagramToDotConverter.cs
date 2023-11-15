using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFD.Model;
using DFD.Model.Interfaces;

namespace DFD.GraphvizConverter
{
    public class DiagramToDotConverter
    {
        private IGraphEntity GetRoot(IDiagram diagram)
        {
            IGraphEntity Root = diagram.Entities.First();
            while (Root.Parent is not null)
            {
                Root = Root.Parent;
            }
            return Root;
        }

        private string RepresentNode(IGraphEntity node, string code)
        {
            if (node.Children.Count == 0) // Draw only leaves
                code += $"{node.FullEntityName.Replace('.', '_')}; \n";
            else
            {
                foreach (var child in node.Children)
                {
                    code += $"subgraph {child.FullEntityName.Replace('.', '_')} \n" +
                            $"{{ label={child.FullEntityName.Replace('.','_')} \n " +
                            $"cluster=True \n";
                    code = RepresentNode(child, code);
                    code += "} \n";
                }
            }
            return code;
        }

        public string ToDot(IDiagram diagram)
        {
            string code = "digraph { ";
            code = RepresentNode(GetRoot(diagram), code);
            foreach (var flow in diagram.Flows)
            {
                code += $"{flow.Source.FullEntityName.Replace('.', '_')} -> {flow.Target.FullEntityName.Replace('.', '_')}; \n";
            }

            code += " } \n";
            return code;
        }
    }
}
