using System.Globalization;
using System.Numerics;
using DFD.GraphConverter.ViewModelImplementation;
using DFD.Model.Interfaces;
using DFD.ViewModel.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DFD.GraphConverter;

public class JsonToGraphParser
{
    public VisualGraph CreateGraphFrom(string json, IGraph<ICollapsableGraphNode> graph)
    {
        JObject graphObj = JsonConvert.DeserializeObject<JObject>(json);

        var visualNodes = new List<IVisualGraphNode>();

        foreach (JObject jsonNode in graphObj["objects"])
        {

            IList<Vector2> points = new List<Vector2>();
            foreach (var tuple in jsonNode["_draw_"][1]["points"])
            {
                points.Add(new Vector2((float)tuple[0], (float)tuple[1]));
            }

            var graphNode = graph.Root.FindMatchingNode(jsonNode["name"].ToString().Replace("_", "."), false);

            visualNodes.Add(new VisualGraphNode()
            {
                Node = graphNode.Data,
                DrawPoints = points,
                DrawOrder = graphNode.FullNodeName.ToCharArray().Count(x => x == '.')
            });
        }
        
        visualNodes.Sort((a, b) => a.DrawOrder.CompareTo(b.DrawOrder));

        var boundingBoxOfGraph = graphObj["bb"]
            .ToString()
            .Split(',')
            .Select(x => float.Parse(x, new NumberFormatInfo() { NumberDecimalSeparator = "." }))
            .ToArray();

        var graphSize = new Vector2(boundingBoxOfGraph[2], boundingBoxOfGraph[3]);

        return new VisualGraph()
        {
            Size = graphSize,
            Nodes = visualNodes
        };
    }
}