namespace DFD.GraphvizConverter;

internal class LocalFolderStrategy : IProcessStartingStrategy
{
    private string _folderName;
    private Dictionary<string, string>? _envVariables;
    public LocalFolderStrategy(string folderName, Dictionary<string, string>? envVariables)
    {
        _folderName = folderName;
        _envVariables = envVariables;
    }
    public byte[] LayoutAndRender(string graph, string layoutAlgorithm, string outputFormat)
    {
        try
        {
            string exePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            GraphvizProcess s = new GraphvizProcess(
                Path.Combine(exePath, "graphviz", _folderName, "dot"),
                Path.Combine(exePath, "graphviz", _folderName),
                _envVariables);
            return s.Render(graph, layoutAlgorithm, outputFormat);
        }
        catch (Exception ex)
        {
            throw new GraphvizErrorException(GraphvizErrorException.InstallationType.Local, ex);
        }
    }
}