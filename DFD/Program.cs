using System;
using System.Diagnostics;
using System.IO;
using System.Reflection.Emit;
using System.Text;
using DFD.GraphvizConverter;
using DFD.Interpreter;
using DFD.Interpreter.ModelImplementations;
using DFD.Model.Interfaces;

class Program
{
    static void Main()
    {
        Interpreter interpreter = new Interpreter();

        var dfdString = File.ReadAllText("documentation.dfd");

        IGraph<GraphNodeData> graph = interpreter.ToDiagram(dfdString);

        foreach (var child in graph.Root.Children)
        {
            Console.WriteLine($"Entity {child.FullEntityName}");
        }

        foreach (var flow in graph.Flows)
        {
            Console.WriteLine($"Flow {flow.Source.FullEntityName} --> {flow.Target.FullEntityName}");
        }

        var dotCode = new DiagramToDotConverter().ToDot(graph);

        Console.WriteLine(dotCode);

        // Path to the Graphviz 'dot' executable
        string dotPath = @"C:\Program Files\Graphviz\bin\dot.exe"; // Adjust this path based on your Graphviz installation

        // Create a temporary file for DOT code
        string dotFilePath = Path.Combine(Path.GetTempPath(), "graph.dot");
        File.WriteAllText(dotFilePath, dotCode);

        // Create a temporary file for the output image
        string outputImagePath = "output.png";


        using (Process process = new Process())
        {
            process.StartInfo.FileName = dotPath;
            process.StartInfo.Arguments = $"-Tpng -o \"{outputImagePath}\" \"{dotFilePath}\"";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.Start();
            process.WaitForExit();

            // Check for errors
            string errorOutput = process.StandardError.ReadToEnd();
            if (!string.IsNullOrWhiteSpace(errorOutput))
            {
                Console.WriteLine($"Error during graph generation: {errorOutput}");
            }
        }

        Console.WriteLine("END");
        Console.ReadKey();
    }
}