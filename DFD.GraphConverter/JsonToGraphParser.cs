﻿using System.Globalization;
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
        JObject graphObj = JsonConvert.DeserializeObject<JObject>(json);

        var boundingBoxOfGraph = graphObj["bb"]
            .ToString()
            .Split(',')
            .Select(x => float.Parse(x, new NumberFormatInfo() { NumberDecimalSeparator = "." }))
            .ToArray();

        var graphSize = new Vector2(boundingBoxOfGraph[2], boundingBoxOfGraph[3]);


        var visualNodes = new List<IVisualGraphNode>();

        foreach (JObject jsonNode in graphObj["objects"])
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

            for (int i = 0; i < points.Count; i++)
            {
                points[i] = new Vector2(points[i].X, -points[i].Y + graphSize.Y);
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

        
        return new VisualGraph()
        {
            Size = graphSize,
            Nodes = visualNodes,
            LogicalGraph = graph
        };
    }
}