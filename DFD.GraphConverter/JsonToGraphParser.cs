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

        foreach (JObject node in graphObj["objects"])
        {

            IList<Vector2> points = new List<Vector2>();
            foreach (var tuple in node["_draw_"][1]["points"])
            {
                points.Add(new Vector2((float)tuple[0], (float)tuple[1]));
            }

            visualNodes.Add(new VisualGraphNode()
            {
                Node = graph.Root.FindMatchingNode(node["name"].ToString().Replace("_", "."), false).Data,
                DrawPoints = points,
                PointConnectionType = PointConnectionType.Straight,
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