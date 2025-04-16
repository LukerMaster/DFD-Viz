using DFD.GraphvizConverter.API;

namespace DFD.GraphvizConverter;

internal class GraphvizRunner : IGraphvizRunner
{

    private IProcessStartingStrategy _processStartingStrategy;

    internal GraphvizRunner(IProcessStartingStrategy processStartingStrategy)
    {
        _processStartingStrategy = processStartingStrategy;
    }
    
    private byte[] GetGraphInMemory(string dotCode, string outputType)
    {
        return _processStartingStrategy.LayoutAndRender(dotCode, null, outputType);
    }
    public string GetGraphAsJson(string dotCode)
    {
        return System.Text.Encoding.UTF8.GetString(GetGraphInMemory(dotCode, "json"));
    }

    public byte[] GetGraphAsPng(string dotCode)
    {
        return GetGraphInMemory(dotCode, "png");
    }

    public void OutputDebugGraphAsPng(string dotCode, string outputPath = "./debug.png")
    {
        _processStartingStrategy.LayoutAndRender( dotCode, outputPath, "png");
    }
}