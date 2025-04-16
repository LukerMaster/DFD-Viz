namespace DFD.GraphvizConverter;

internal interface IProcessStartingStrategy
{
    public byte[] LayoutAndRender(string graph, string? layoutAlgorithm,
        string outputFormat);
}