using System.Diagnostics;
using System.Text;

namespace DFD.GraphvizConverter;

public class GraphvizProcessStarter
{
    public byte[] LayoutAndRender(string graphFilePath, string graph, string outputFilePath, string layoutAlgorithm, string outputFormat,
            params string[] extraCommandLineFlags)
        {
            if (graphFilePath == null && graph == null)
            {
                throw new ArgumentException($"Arguments {nameof(graphFilePath)} and {nameof(graph)} cannot be null at the same time.");
            }

            string arguments = BuildCommandLineArguments(graphFilePath, outputFilePath, layoutAlgorithm, outputFormat, extraCommandLineFlags);

            string exePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            
            var graphVizProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    FileName = Path.Combine(exePath, "graphviz", "ubuntu", "dot"),
                    Arguments = arguments,
                    WorkingDirectory = Path.Combine(exePath, "graphviz", "ubuntu"),
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
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
    
    public static string BuildCommandLineArguments(string inputFilePath, string outputFilePath, string layout, string format,
        params string[] extraCommandLineFlags)
    {
        var argumentsBuilder = new StringBuilder();

        if (layout != null)
        {
            argumentsBuilder.Append(" -K").Append(layout);
        }

        if (format != null)
        {
            argumentsBuilder.Append(" -T").Append(format);
        }

        if (outputFilePath != null)
        {
            argumentsBuilder.Append(" -o\"").Append(outputFilePath).Append('\"');
        }

        foreach (string extraFlag in extraCommandLineFlags)
        {
            argumentsBuilder.Append(' ').Append(extraFlag);
        }

        if (inputFilePath != null)
        {
            argumentsBuilder.Append(" \"").Append(inputFilePath).Append('\"');
        }

        return argumentsBuilder.ToString();
    }
}