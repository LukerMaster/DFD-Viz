﻿using System.Globalization;
using System.Numerics;
using DFD.DataStructures.Interfaces;
using DFD.GraphConverter.ViewModelImplementation;
using DFD.ViewModel.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ICollapsibleNodeData = DFD.DataStructures.Interfaces.ICollapsibleNodeData;

namespace DFD.GraphConverter;

internal class JsonToGraphParser
{
    public IVisualGraph CreateGraphFrom(string json, IGraph<ICollapsibleNodeData> graph)
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
        var visualTexts = GetTexts(rootJsonObject);


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

    private IReadOnlyList<IVisualText> GetTexts(JObject rootJsonObject)
    {
        List<IVisualText> visualTexts = new List<IVisualText>();

        if (rootJsonObject["objects"] is not null)
        {
            foreach (var graphObj in rootJsonObject["objects"])
            {
                if (graphObj["_ldraw_"] is not null)
                {
                    visualTexts.Add(GetText(graphObj["_ldraw_"]));
                }
            }
        }

        if (rootJsonObject["edges"] is not null)
        {
            foreach (var graphObj in rootJsonObject["edges"])
            {
                if (graphObj["_ldraw_"] is not null)
                {
                    visualTexts.Add(GetText(graphObj["_ldraw_"]));
                }
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
                text.Width = float.Parse(operation["width"].ToString());
            }
        }

        return text;
    }

    private IReadOnlyList<IVisualObject> GetArrowHeads(JObject rootJsonObject)
    {
        var arrowHeads = new List<IVisualObject>();

        if (rootJsonObject["edges"] is null) return arrowHeads;

        foreach (var edge in rootJsonObject["edges"])
        {
            var vo = TryGetVisualObjectFrom(edge, "_hdraw_");
            if (vo is not null)
                arrowHeads.Add(vo);
        }
        foreach (var edge in rootJsonObject["edges"])
        {
            var vo = TryGetVisualObjectFrom(edge, "_tdraw_");
            if (vo is not null)
                arrowHeads.Add(vo);
        }

        return arrowHeads;
    }

    private IReadOnlyList<IVisualObject> GetFlows(JObject rootJsonObject)
    {
        var visualFlows = new List<IVisualObject>();

        if (rootJsonObject["edges"] is null) return visualFlows;

        foreach (var edge in rootJsonObject["edges"])
        {
            var vo = TryGetVisualObjectFrom(edge, "_draw_");
            if (vo is not null)
                visualFlows.Add(vo);
        }

        return visualFlows;
    }

    private VisualObject? TryGetVisualObjectFrom(JToken graphObj, string definitionName, bool isClosed = false)
    {
        if (graphObj[definitionName] is null) return null;

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
        
        DrawTechnique drawTechnique = drawDefinition["op"].ToString() == "b"
            ? DrawTechnique.Bezier
            : DrawTechnique.Straight;

        var pointList = new List<Vector2>();

        foreach (var point in drawDefinition["points"])
        {
            pointList.Add(new Vector2((float)point[0], (float)point[1]));
        }

        
        var points = pointList;
        return new VisualObject()
        {
            Points = pointList,
            IsClosed = isClosed,
            DrawTechnique = drawTechnique,
        };
    }

    private IReadOnlyList<IVisualGraphNode> GetNodes(IGraph<ICollapsibleNodeData> graph, JObject rootJsonObject)
    {
        var visualNodes = new List<IVisualGraphNode>();

        foreach (JObject jsonNode in rootJsonObject["objects"])
        {
            VisualObject? vo = TryGetVisualObjectFrom(jsonNode, "_draw_", true);

            if (vo is not null)
            {
                var graphNode = graph.Root.FindMatchingNode(jsonNode["name"].ToString());
                vo.DrawOrder = graphNode.GetAncestorCount();

                visualNodes.Add(new VisualGraphNode()
                {
                    Node = graphNode.Data,
                    VisualObject = vo
                });
            }
        }

        visualNodes.Sort((a, b) => a.VisualObject.DrawOrder.CompareTo(b.VisualObject.DrawOrder));
        return visualNodes;
    }
}