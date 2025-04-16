namespace DFD.GraphvizConverter;

internal class SystemwiseStrategy : IProcessStartingStrategy
{
    public byte[] LayoutAndRender(string graph, string layoutAlgorithm, string outputFormat)
    {
        try
        {
            GraphvizProcess s = new GraphvizProcess("dot", null, null);
            return s.Render(graph, layoutAlgorithm, outputFormat);
        }
        catch (Exception ex)
        {
            throw new GraphvizErrorException(GraphvizErrorException.InstallationType.Systemwise, ex);
        }
    }
}