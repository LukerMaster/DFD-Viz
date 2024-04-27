namespace DFD.GraphvizConverter;

public class GraphvizErrorException : Exception
{
    public enum InstallationType
    {
        Local,
        Systemwise
    }
    public InstallationType Type { get; }

    public GraphvizErrorException(InstallationType type)
    {
        Type = type;
    }

}