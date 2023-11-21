namespace DFD.GraphvizConverter;

public class GraphvizRunner
{
    private byte[] GetGraphWithTempFiles(string dotCode, string outputType)
    {
        string fileName = "temp-output-graphviz-" + Guid.NewGuid().ToString() + "." + outputType;
        string tempFilePath = Path.Combine(Path.GetTempPath(), fileName);
        var runner = new GraphVizNet.GraphViz();
        runner.LayoutAndRender(null, dotCode, tempFilePath, null, outputType);
        byte[] fileOutput = File.ReadAllBytes(tempFilePath);
        return fileOutput;
    }

    private byte[] GetGraphInMemory(string dotCode, string outputType)
    {
        var runner = new GraphVizNet.GraphViz();
        return runner.LayoutAndRender(null, dotCode, null, null, outputType);
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
        var runner = new GraphVizNet.GraphViz();
        runner.LayoutAndRender(null, dotCode, outputPath, null, "png");
    }
}