using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFD.Model.Interfaces;

namespace DFD.GraphvizConverter
{
    public class DiagramToDotConverter
    {
        public string ToDot(IDiagram diagram)
        {
            string code = "digraph { ";
            foreach (var node in diagram.Entities)
            {
                if (node.Children.Count == 0) // Draw only leaves
                    code += $"{node.FullEntityName.Replace('.', '_')}; \n";
            }

            foreach (var flow in diagram.Flows)
            {
                code += $"{flow.Source.FullEntityName.Replace('.', '_')} -> {flow.Target.FullEntityName.Replace('.', '_')}; \n";
            }

            code += " }";
            return code;
        }
    }
}
