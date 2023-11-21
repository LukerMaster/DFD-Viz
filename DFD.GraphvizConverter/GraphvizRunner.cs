namespace DFD.GraphvizConverter;

public class GraphvizRunner
{
    private byte[] GetGraph(string dotCode, string outputType)
    {
        string fileName = "temp-output-graphviz-" + Guid.NewGuid().ToString() + "." + outputType;
        string tempFilePath = Path.Combine(Path.GetTempPath(), fileName);
        var runner = new GraphVizNet.GraphViz();
        runner.LayoutAndRender(null, dotCode, tempFilePath, null, outputType);
        byte[] fileOutput = File.ReadAllBytes(tempFilePath);
        return fileOutput;
    }
    public string GetGraphAsJson(string dotCode)
    {
        return System.Text.Encoding.UTF8.GetString(GetGraph(dotCode, "json"));
    }

    public byte[] GetGraphAsPng(string dotCode)
    {
        return GetGraph(dotCode, "png");
    }

    public void OutputDebugGraphAsPng(string dotCode, string outputPath = "./debug.png")
    {
        var runner = new GraphVizNet.GraphViz();
        runner.LayoutAndRender(null, dotCode, outputPath, null, "png");
    }
}