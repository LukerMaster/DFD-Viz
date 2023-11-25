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
        var visualTexts = GetAllTexts(rootJsonObject);


        return new VisualGraph()
        {
            Size = graphSize,
            Nodes = visualNodes,
            LogicalGraph = graph,
            Flows = visualFlows,
            ArrowHeads = arrowHeads,
            TextLabels = visualTexts
        };
    }

    private IReadOnlyList<IVisualText> GetAllTexts(JObject rootJsonObject)
    {
        List<IVisualText> visualTexts = new List<IVisualText>();
        foreach (var graphObj in rootJsonObject["objects"])
        {
            if(graphObj["_ldraw_"] is not null)
            {
                visualTexts.Add(GetText(graphObj["_ldraw_"]));
            }
        }
        foreach (var graphObj in rootJsonObject["edges"])
        {
            if (graphObj["_ldraw_"] is not null)
            {
                visualTexts.Add(GetText(graphObj["_ldraw_"]));
            }
        }

        return visualTexts;
    }

    private IVisualText GetText(JToken textDrawDefinition)
    {
        VisualText text = new VisualText();

        foreach (var operation in textDrawDefinition)
        {
            if (operation["op"]?.ToString() == "F")
            {
                text.FontSize = (float)operation["size"];
            }

            if (operation["op"]?.ToString() == "T")
            {
                text.Origin = operation["align"].ToString() == "c" ? new Vector2(0.5f, 0.5f) : Vector2.Zero;
                text.Position = new Vector2((float)operation["pt"][0], (float)operation["pt"][1]);
                text.Text = operation["text"].ToString();
            }
        }

        return text;
    }

    private IReadOnlyList<IVisualObject> GetArrowHeads(JObject rootJsonObject)
    {
        var arrowHeads = new List<IVisualObject>();

        foreach (var edge in rootJsonObject["edges"])
        {
            arrowHeads.Add(GetVisualObjectFrom(edge, "_hdraw_"));
        }

        return arrowHeads;
    }

    private IReadOnlyList<IVisualObject> GetFlows(JObject rootJsonObject)
    {
        var visualFlows = new List<IVisualObject>();

        foreach (var edge in rootJsonObject["edges"])
        {
            visualFlows.Add(GetVisualObjectFrom(edge, "_draw_"));
        }

        return visualFlows;
    }

    private VisualObject GetVisualObjectFrom(JToken graphObj, string definitionName, bool isClosed = false)
    {
        foreach (var drawDefinition in graphObj[definitionName])
        {
            if (drawDefinition["points"] is not null)
            {
               return ParseDrawDefinition(drawDefinition, isClosed);
            }
        }
        throw new Exception("Unsupported shape type.");
    }

    private VisualObject ParseDrawDefinition(JToken drawDefinition, bool isClosed = false)
    {
        VisualObject visualObject = new VisualObject();
        visualObject.DrawTechnique = drawDefinition["op"].ToString() == "b"
            ? DrawTechnique.Bezier
            : DrawTechnique.Straight;

        var pointList = new List<Vector2>();

        foreach (var point in drawDefinition["points"])
        {
            pointList.Add(new Vector2((float)point[0], (float)point[1]));
        }

        visualObject.Points = pointList;
        visualObject.IsClosed = isClosed;
        return visualObject;
    }

    private IReadOnlyList<IVisualGraphNode> GetNodes(IGraph<ICollapsableGraphNode> graph, JObject rootJsonObject)
    {
        var visualNodes = new List<IVisualGraphNode>();

        foreach (JObject jsonNode in rootJsonObject["objects"])
        {
            VisualObject vo = GetVisualObjectFrom(jsonNode, "_draw_", true);
            
            var graphNode = graph.Root.FindMatchingNode(jsonNode["name"].ToString().Replace("_", "."), false);
            vo.DrawOrder = graphNode.FullNodeName.ToCharArray().Count(x => x == '.');

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