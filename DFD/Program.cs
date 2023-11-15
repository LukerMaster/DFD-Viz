using System;
using System.Diagnostics;
using System.IO;
using System.Reflection.Emit;
using System.Text;
using DFD.GraphvizConverter;
using DFD.Interpreter;
using DFD.Model.Interfaces;

class Program
{
    static void Main()
    {
        Interpreter interpreter = new Interpreter();

        var dfdString = File.ReadAllText("disambiguouation.dfd");

        IDiagram diagram = interpreter.ToDiagram(dfdString);

        foreach (var entity in diagram.Entities)
        {
            Console.WriteLine($"Entity {entity.FullEntityName}");
        }

        foreach (var flow in diagram.Flows)
        {
            Console.WriteLine($"Flow {flow.Source.FullEntityName} --> {flow.Target.FullEntityName}");
        }

        var dotCode = new DiagramToDotConverter().ToDot(diagram);

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