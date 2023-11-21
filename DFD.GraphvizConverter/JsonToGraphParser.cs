using System.Globalization;
using System.Numerics;
using System.Text.Json;
using DataStructure.NamedTree;
using DFD.GraphvizConverter.ViewModelImplementation;
using DFD.Model.Interfaces;
using DFD.ViewModel.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DFD.GraphvizConverter;

public class JsonToGraphParser
{
    public VisualGraph CreateGraphFrom(string json, IGraph<ICollapsableGraphNode> graph)
    {
        JObject graphObj = JsonConvert.DeserializeObject<JObject>(json);

        var visualNodes = new List<IVisualGraphNode>();
        //var visualFlows = new List<IVisualFlow>();

        Console.WriteLine("\n\nENUMERATE:\n\n");

        foreach (JObject node in graphObj["objects"])
        {
            var boundingBox = node["bb"]
                .ToString()
                .Split(",")
                .Select(x => float.Parse(x, new NumberFormatInfo() { NumberDecimalSeparator = "." }))
                .ToArray();


            visualNodes.Add(new VisualGraphNode()
            {
                Node = graph.Root.FindMatchingNode(node["name"].ToString().Replace("_", "."), false).Data,
                Position = new Vector2(boundingBox[0], boundingBox[2]),
                Size = new Vector2(boundingBox[1], boundingBox[3]),
                Symbol = DisplayType.Rectangle
            });
        }

        //foreach (var edge in graphObj["edges"])
        //{
        //    visualFlows.Add(new VisualFlow()
        //    {
        //        Label = graph.Flows.
        //    });
        //}

        return new VisualGraph()
        {
            Nodes = visualNodes
        };
    }
}