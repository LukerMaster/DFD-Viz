using DFD.GraphConverter;
using DFD.Interpreter;
using DFD.Model.Interfaces;

class Program
{
    static void Main()
    {
        Interpreter interpreter = new Interpreter();

        var dfdString = File.ReadAllText("documentation.dfd");

        IGraph<IGraphNodeData> graph = interpreter.ToDiagram(dfdString);

        foreach (var child in graph.Root.Children)
        {
            Console.WriteLine($"Entity {child.FullNodeName}");
        }

        foreach (var flow in graph.Flows)
        {
            Console.WriteLine($"Flow {flow.Source.FullNodeName} --> {flow.Target.FullNodeName}");
        }

        var multilevelGraph = new MultilevelGraphConverter().CreateMultiLevelGraphOutOf(graph);
        var visualGraph = new VisualGraphCreator().GetVisualGraph(multilevelGraph);


        Console.WriteLine("END");
        Console.ReadKey();
    }
}