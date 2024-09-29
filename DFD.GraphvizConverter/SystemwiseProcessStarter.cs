namespace DFD.GraphvizConverter;

internal class SystemwiseProcessStarter : IGraphvizProcessStarter
{
    public byte[] LayoutAndRender(string graph, string layoutAlgorithm, string outputFormat,
        params string[] extraCommandLineFlags)
    {
        try
        {
            StandardProcessStarter s = new StandardProcessStarter("dot", null);
            return s.Render(graph, layoutAlgorithm, outputFormat, extraCommandLineFlags);
        }
        catch (Exception ex)
        {
            throw new GraphvizErrorException(GraphvizErrorException.InstallationType.Systemwise, ex);
        }
    }
}