using System.Diagnostics;
using System.Text;

namespace DFD.GraphvizConverter;

internal class GraphvizProcess
{
    private readonly string? _workingDirectory;
    private readonly string _fileName;
    private readonly Dictionary<string, string>? _envVariables;
    
    internal GraphvizProcess(string fileName, string? workingDirectory, Dictionary<string, string>? envVariables)
    {
        _fileName = fileName;
        _workingDirectory = workingDirectory;
        _envVariables = envVariables;
    }

    internal byte[] Render(string graph, string layoutAlgorithm, string outputFormat)
    {
        if (graph == null)
        {
            throw new ArgumentException($"Argument {nameof(graph)} cannot be null.");
        }
        string arguments = CommandArgumentsBuilder.BuildCommandLineArguments(layoutAlgorithm, outputFormat);
        
        var graphVizProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                FileName = _fileName,
                WorkingDirectory = _workingDirectory,
                Arguments = arguments,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                StandardInputEncoding = new UTF8Encoding(false), // BOM (Byte order mark) breaks Graphviz, so I explicitly disable it.
                StandardErrorEncoding = Encoding.UTF8,
                StandardOutputEncoding = Encoding.UTF8,
                UseShellExecute = false
            }
        };

        if (_envVariables != null)
        {
            foreach (var envVariable in _envVariables)
            {
                graphVizProcess.StartInfo.EnvironmentVariables[envVariable.Key] = envVariable.Value;
            }    
        }
        graphVizProcess.Start();
        
        graphVizProcess.StandardInput.Write(graph);
        graphVizProcess.StandardInput.Close();
        
        byte[] result;
        using (Stream baseStream = graphVizProcess.StandardOutput.BaseStream)
        using (var memoryStream = new MemoryStream())
        {
            baseStream.CopyTo(memoryStream);
            result = memoryStream.ToArray();
        }

        string errorAndWarningMessages = graphVizProcess.StandardError.ReadToEnd();

        graphVizProcess.WaitForExit(40000);

        if (graphVizProcess.ExitCode != 0)
        {
            throw new Exception($"dot process exited with code: {graphVizProcess.ExitCode} and with errors: {errorAndWarningMessages}");
        }
        return result;
    }

}