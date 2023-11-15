﻿using System;
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
        private IGraphEntity GetRoot(IGraph graph)
        {
            IGraphEntity Root = graph.Entities.First();
            while (Root.Parent is not null)
            {
                Root = Root.Parent;
            }
            return Root;
        }

        private string RepresentNode(IGraphEntity node, string code, bool useDisplayNames = false)
        {
            foreach (var child in node.Children)
            {
                // If child node has children, draw them as subgraphs
                if (child.Children.Count > 0)
                {
                    code += $"subgraph {child.FullEntityName.Replace('.', '_')} \n" +
                            $"{{ label={child.FullEntityName.Replace('.', '_')} \n " +
                            $"cluster=True \n";
                    code = RepresentNode(child, code);
                    code += "} \n";
                }
                // If child node is a leaf, draw it as node
                else
                {
                    code += $"{child.FullEntityName.Replace('.', '_')}; \n";
                }
                
            }
            
            return code;
        }

        public string ToDot(IGraph graph)
        {
            string code = "digraph { ";
            code = RepresentNode(GetRoot(graph), code);
            foreach (var flow in graph.Flows)
            {
                code += $"{flow.Source.FullEntityName.Replace('.', '_')} -> {flow.Target.FullEntityName.Replace('.', '_')}; \n";
            }

            code += " } \n";
            return code;
        }
    }
}
