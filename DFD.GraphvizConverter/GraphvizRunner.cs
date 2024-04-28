namespace DFD.GraphvizConverter;

public class GraphvizRunner
{

    private IGraphvizProcessStarter _processStarter;

    internal GraphvizRunner(IGraphvizProcessStarter processStarter)
    {
        _processStarter = processStarter;
    }
    
    private byte[] GetGraphInMemory(string dotCode, string outputType)
    {
        return _processStarter.LayoutAndRender(dotCode, null, outputType);
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
        _processStarter.LayoutAndRender( dotCode, outputPath, null, "png");
    }
}