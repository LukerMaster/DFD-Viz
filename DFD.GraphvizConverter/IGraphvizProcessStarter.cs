namespace DFD.GraphvizConverter;

internal interface IGraphvizProcessStarter
{
    public byte[] LayoutAndRender(string graph, string layoutAlgorithm,
        string outputFormat,
        params string[] extraCommandLineFlags);
}