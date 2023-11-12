using System;
using System.Diagnostics;
using System.IO;
using System.Text;

class Program
{
    static void Main()
    {
        string dotSource = "digraph G { ProcessA -> ProcessB }"; 

        string jsonOutput = GenerateGraphJson(dotSource);

        Console.WriteLine(jsonOutput);
        Console.ReadKey();
    }

    static string GenerateGraphJson(string dotSource)
    {
        using (Process process = new Process())
        {
            process.StartInfo.FileName = "dot";
            process.StartInfo.Arguments = "-Tjson";
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.Start();

            using (StreamWriter sw = process.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.Write(dotSource);
                }
            }

            using (StreamReader sr = process.StandardOutput)
            {
                return sr.ReadToEnd();
            }
        }
    }
}