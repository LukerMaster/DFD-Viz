using System;
using System.Diagnostics;
using System.IO;
using System.Reflection.Emit;
using System.Text;
using DFD.GraphvizConverter;
using DFD.Interpreter;
using DFD.Model.Interfaces;
using DFD.ModelImplementations;

class Program
{
    static void Main()
    {
        Interpreter interpreter = new Interpreter();

        var dfdString = File.ReadAllText("documentation.dfd");

        IGraph<IGraphNodeData> graph = interpreter.ToDiagram(dfdString);

        foreach (var child in graph.Root.Children)
        {
            Console.WriteLine($"Entity {child.FullEntityName}");
        }

        foreach (var flow in graph.Flows)
        {
            Console.WriteLine($"Flow {flow.Source.FullEntityName} --> {flow.Target.FullEntityName}");
        }

        var dotCode = new VisualGraphCreator().GetVisualGraph(graph);

        

        Console.WriteLine("END");
        Console.ReadKey();
    }
}