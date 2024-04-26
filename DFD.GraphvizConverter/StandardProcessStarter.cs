using System.Diagnostics;
using System.Text;

namespace DFD.GraphvizConverter;

internal class StandardProcessStarter
{
    private readonly string? _workingDirectory;
    private readonly string _fileName;

    internal StandardProcessStarter(string FileName, string? WorkingDirectory)
    {
        _fileName = FileName;
        _workingDirectory = WorkingDirectory;
    }

    internal byte[] Render(string graph, string layoutAlgorithm, string outputFormat,
        params string[] extraCommandLineFlags)
    {
        if (graph == null)
        {
            throw new ArgumentException($"Argument {nameof(graph)} cannot be null.");
        }
        string arguments = CommandArgumentsBuilder.BuildCommandLineArguments(layoutAlgorithm, outputFormat, extraCommandLineFlags);

        string exePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
        
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

        graphVizProcess.Start();

        if (graph != null)
        {
            graphVizProcess.StandardInput.Write(graph);
            graphVizProcess.StandardInput.Close();
        }

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