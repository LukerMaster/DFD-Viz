using System.Diagnostics;
using System.Text;

namespace DFD.GraphvizConverter;

internal class WindowsProcessStarter : IGraphvizProcessStarter
{
    public byte[] LayoutAndRender(string graph, string layoutAlgorithm, string outputFormat,
        params string[] extraCommandLineFlags)
    {
        string exePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
        StandardProcessStarter s = new StandardProcessStarter(
            Path.Combine(exePath, "graphviz", "win64", "dot"),
            Path.Combine(exePath, "graphviz", "win64"));
        return s.Render(graph, layoutAlgorithm, outputFormat, extraCommandLineFlags);
    }
}