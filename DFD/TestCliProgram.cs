using DFD.GraphConverter;
using DFD.Parsing;
using DFD.Model.Interfaces;

class TestCliProgram
{
    static void Main()
    {
        Interpreter interpreter = new Interpreter();

        var dfdString = File.ReadAllText("documentation.dfd");

        IGraph<IGraphNodeData> graph = interpreter.ToDiagram(dfdString);

        foreach (var child in graph.Root.Children)
        {
            Console.WriteLine($"Node {child.FullNodeName}");
        }

        foreach (var flow in graph.Flows)
        {
            Console.WriteLine($"Flow {flow.SourceNodeName} --> {flow.TargetNodeName}");
        }

        var multilevelGraph = new MultilevelGraphConverter().ToMultilevelGraph(graph);
        var visualGraph = new VisualGraphCreator().GetVisualGraph(multilevelGraph);


        Console.WriteLine("END");
        Console.ReadKey();
    }
}