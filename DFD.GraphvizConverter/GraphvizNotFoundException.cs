namespace DFD.GraphvizConverter;

public class GraphvizNotFoundException : Exception
{
    public string CommandToInstallGraphviz { get; }

    public GraphvizNotFoundException(string commandToInstallGraphviz)
    {
        CommandToInstallGraphviz = commandToInstallGraphviz;
    }

}