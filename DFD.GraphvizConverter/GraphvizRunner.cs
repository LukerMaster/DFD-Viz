namespace DFD.GraphvizConverter;

public class GraphvizRunner
{
    private byte[] GetGraph(string dotCode, string outputType)
    {
        var runner = new GraphVizNet.GraphViz();
        byte[] output = runner.LayoutAndRender(null, dotCode, null, null, "json");
        return output;
    }
    public string GetGraphAsJson(string dotCode)
    {
        return System.Text.Encoding.UTF8.GetString(GetGraph(dotCode, "json"));
    }

    public byte[] GetGraphAsPng(string dotCode)
    {
        return GetGraph(dotCode, "png");
    }

    internal void OutputDebugGraphAsPng(string dotCode, string outputPath = "./debug.png")
    {
        var runner = new GraphVizNet.GraphViz();
        runner.LayoutAndRender(null, dotCode, outputPath, null, "png");
    }
}