using DataStructure.NamedTree;
using System.Diagnostics;

namespace DFD.GraphvizConverter;

public class GraphvizRunner
{
    public string GetGraphAsJson(string dotCode)
    {
        
        // Create a temporary file for DOT code
        string dotFilePath = Path.Combine(Path.GetTempPath(), "graph.dot");
        File.WriteAllText(dotFilePath, dotCode);

        var runner = new GraphVizNet.GraphViz();

        byte[] output = runner.LayoutAndRender(null, dotCode, null, null, "json");

        return System.Text.Encoding.UTF8.GetString(output);
    }
}