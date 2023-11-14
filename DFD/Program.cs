using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using DFD.Interpreter;
using DFD.Model.Interfaces;

class Program
{
    static void Main()
    {
        Interpreter interpreter = new Interpreter();

        var dfdString = File.ReadAllText("example-ml.dfd");

        IDiagram diagram = interpreter.ToDiagram(dfdString);

        foreach (var entity in diagram.Entities)
        {
            Console.WriteLine($"Entity {entity.FullEntityName}");
        }

        foreach (var flow in diagram.Flows)
        {
            Console.WriteLine($"Flow {flow.Source.FullEntityName} --> {flow.Target.FullEntityName}");
        }

        Console.WriteLine("END");
        Console.ReadKey();
    }
}