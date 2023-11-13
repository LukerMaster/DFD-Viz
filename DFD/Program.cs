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

        var dfdString = File.ReadAllText("example-simple.dfd");

        IDiagram diagram = interpreter.ToDiagram(dfdString);

        foreach (var entity in diagram.Entities)
        {
            Console.WriteLine(entity);
        }

        Console.WriteLine("END");
        Console.ReadKey();
    }
}