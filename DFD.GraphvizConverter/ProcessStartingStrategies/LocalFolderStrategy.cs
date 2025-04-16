namespace DFD.GraphvizConverter;

internal class LocalFolderStrategy(string fileName, string workingDirectory, Dictionary<string, string>? envVariables)
    : IProcessStartingStrategy
{
    public byte[] LayoutAndRender(string graph, string layoutAlgorithm, string outputFormat)
    {
        try
        {
            GraphvizProcess s = new GraphvizProcess(fileName, workingDirectory, envVariables);
            return s.Render(graph, layoutAlgorithm, outputFormat);
        }
        catch (Exception ex)
        {
            throw new GraphvizErrorException(GraphvizErrorException.InstallationType.Local, ex);
        }
    }
}