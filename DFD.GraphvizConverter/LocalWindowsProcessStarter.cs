namespace DFD.GraphvizConverter;

internal class LocalWindowsProcessStarter : IGraphvizProcessStarter
{
    public byte[] LayoutAndRender(string graph, string layoutAlgorithm, string outputFormat,
        params string[] extraCommandLineFlags)
    {
        try
        {
            string exePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            StandardProcessStarter s = new StandardProcessStarter(
                Path.Combine(exePath, "graphviz", "win64", "dot"),
                Path.Combine(exePath, "graphviz", "win64"));
            return s.Render(graph, layoutAlgorithm, outputFormat, extraCommandLineFlags);
        }
        catch (Exception ex)
        {
            throw new GraphvizErrorException(GraphvizErrorException.InstallationType.Local, ex);
        }
    }
}