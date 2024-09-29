using DFD.DataStructures.Interfaces;
using DFD.GraphConverter;
using DFD.GraphvizConverter;
using DFD.Parsing;

class TestCliProgram
{
    static void Main()
    {
        
        Interpreter interpreter = new Interpreter(new NodeDataFactory());

        var dfdString = File.ReadAllText("documentation.dfd");

        IGraph<INodeData> graph = interpreter.ToDiagram(dfdString);

        foreach (var child in graph.Root.Children)
        {
            Console.WriteLine($"Node {child.FullPath}");
        }

        foreach (var flow in graph.Flows)
        {
            Console.WriteLine($"Flow {flow.Source} --> {flow.Target}");
        }

        var multilevelGraph = new MultilevelGraphConverter().ToMultilevelGraph(graph);
        var visualGraph = new VisualGraphCreator(new GraphvizRunnerFactory(Environment.OSVersion.Platform).CreateRunner()).GetVisualGraph(multilevelGraph);


        Console.WriteLine("END");
        Console.ReadKey();
    }
}