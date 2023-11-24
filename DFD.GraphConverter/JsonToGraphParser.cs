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
    public IVisualGraph CreateGraphFrom(string json, IGraph<ICollapsableGraphNode> graph)
    {
        JObject rootJsonObject = JsonConvert.DeserializeObject<JObject>(json);

        var boundingBoxOfGraph = rootJsonObject["bb"]
            .ToString()
            .Split(',')
            .Select(x => float.Parse(x, new NumberFormatInfo() { NumberDecimalSeparator = "." }))
            .ToArray();

        var graphSize = new Vector2(boundingBoxOfGraph[2], boundingBoxOfGraph[3]);


        var visualNodes = GetNodes(graph, rootJsonObject);
        var visualFlows = GetFlows(rootJsonObject);
        var arrowHeads = GetArrowHeads(rootJsonObject);


        return new VisualGraph()
        {
            Size = graphSize,
            Nodes = visualNodes,
            LogicalGraph = graph,
            Flows = visualFlows,
            ArrowHeads = arrowHeads
        };
    }

    private List<IVisualObject> GetArrowHeads(JObject rootJsonObject)
    {
        var arrowHeads = new List<IVisualObject>();

        foreach (var edge in rootJsonObject["edges"])
        {
            arrowHeads.Add(ParseDrawDefinition(edge, "_hdraw_"));
        }

        return arrowHeads;
    }

    private List<IVisualObject> GetFlows(JObject rootJsonObject)
    {
        var visualFlows = new List<IVisualObject>();

        foreach (var edge in rootJsonObject["edges"])
        {
            visualFlows.Add(ParseDrawDefinition(edge, "_draw_"));
        }

        return visualFlows;
    }

    private IVisualObject ParseDrawDefinition(JToken edge, string definitionName)
    {
        foreach (var drawDefinition in edge[definitionName])
        {
            if (drawDefinition["points"] is not null)
            {
               return GetDrawDefinition(drawDefinition);
            }
        }
        throw new Exception("Unsupported shape type.");
    }

    private IVisualObject GetDrawDefinition(JToken drawDefinition)
    {
        VisualObject flow = new VisualObject();
        flow.DrawTechnique = drawDefinition["op"].ToString() == "b"
            ? DrawTechnique.Bezier
            : DrawTechnique.Straight;

        var pointList = new List<Vector2>();

        foreach (var point in drawDefinition["points"])
        {
            pointList.Add(new Vector2((float)point[0], (float)point[1]));
        }

        flow.Points = pointList;
        flow.IsClosed = char.IsUpper(drawDefinition["op"].ToString()[0]); // I'm not quite sure if letter being uppercase actually makes it closed.
        return flow;
    }

    private static List<IVisualGraphNode> GetNodes(IGraph<ICollapsableGraphNode> graph, JObject rootJsonObject)
    {
        var visualNodes = new List<IVisualGraphNode>();

        foreach (JObject jsonNode in rootJsonObject["objects"])
        {
            IList<Vector2> points = new List<Vector2>();


            foreach (var drawDefinition in jsonNode["_draw_"])
            {
                if (drawDefinition["points"] is not null && drawDefinition["op"]?.Value<string>() == "p")
                {
                    foreach (var pointDefinition in drawDefinition["points"])
                    {
                        points.Add(new Vector2((float)pointDefinition[0], (float)pointDefinition[1]));
                    }
                }
            }

            var graphNode = graph.Root.FindMatchingNode(jsonNode["name"].ToString().Replace("_", "."), false);

            IVisualObject vo = new VisualObject()
            {
                Points = (IReadOnlyList<Vector2>)points,
                DrawOrder = graphNode.FullNodeName.ToCharArray().Count(x => x == '.'),
                DrawTechnique = DrawTechnique.Straight,
                IsClosed = true,
            };

            visualNodes.Add(new VisualGraphNode()
            {
                Node = graphNode.Data,
                VisualObject = vo
            });
        }

        visualNodes.Sort((a, b) => a.VisualObject.DrawOrder.CompareTo(b.VisualObject.DrawOrder));
        return visualNodes;
    }
}