using DFD.DataStructures.Interfaces;
using DFD.GraphConverter;
using DFD.GraphvizConverter;
using DFD.Parsing;

class TestCliProgram
{
    static void Main()
    {
        
        Interpreter<ICollapsibleNodeData> interpreter = new(new NodeDataFactory());

        var dfdString = File.ReadAllText("documentation.dfd");

        IGraph<ICollapsibleNodeData> graph = interpreter.ToDiagram(dfdString);

        foreach (var child in graph.Root.Children)
        {
            Console.WriteLine($"Node {child.FullPath}");
        }

        foreach (var flow in graph.Flows)
        {
            Console.WriteLine($"Flow {flow.Source} --> {flow.Target}");
        }

        new MultilevelGraphPreparator().TweakCollapsability(graph);
        var visualGraph = new VisualGraphCreator(new GraphvizRunnerFactory(Environment.OSVersion.Platform).CreateRunner()).GetVisualGraph(graph);


        Console.WriteLine("END");
        Console.ReadKey();
    }
}