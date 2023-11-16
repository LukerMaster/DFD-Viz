using System.Diagnostics;
using System.Text.RegularExpressions;
using DataStructure.NamedTree;
using DFD.Interpreter.ModelImplementations;
using DFD.Model;
using DFD.Model.Interfaces;

namespace DFD.Interpreter;

internal class GraphObjectParser
{
    private readonly Dictionary<string, NodeType> ValidDefinitions = new()
    {
        { "Process", NodeType.Process },
        { "Storage", NodeType.Storage },
        { "IO", NodeType.InputOutput },
    };

    public ITreeNode<GraphNodeData>? TryParseEntity(string line, ITreeNode<GraphNodeData> currentParent)
    {
        ITreeNode<GraphNodeData>? entity = null;

        // Split by any amount of whitespace
        var definition = SplitByWhitespace(line);

        var typeName = definition[0];
        var entityName = definition[1];
        var displayedName = definition[2];

        if (ValidDefinitions.ContainsKey(typeName))
        {
            var type = ValidDefinitions[typeName];
            entity = CreateStandardEntity(type, entityName, displayedName, currentParent);
            currentParent.Children.Add(entity);
        }
        
        return entity;
    }


    private static IList<string> SplitByWhitespace(string line)
    {
        Regex regex = new Regex(@"(?:[^\s""]+|""[^""]*"")+", RegexOptions.Compiled);

        // Extract matches
        MatchCollection matches = regex.Matches(line);

        // Convert matches to string array
        string[] result = new string[matches.Count];
        for (int i = 0; i < matches.Count; i++)
        {
            result[i] = matches[i].Value;
        }

        return result;
    }

    public IFlow<T>? TryParseFlow<T>(string statement, ITreeNode<T> currentParent)
    {
        IFlow<T>? flow = null;

        var definition = SplitByWhitespace(statement);

        var entityNameA = definition[0];
        var flowType = definition[1]; // TODO: do something with this value.
        var entityNameB = definition[2];

        var displayedName = String.Empty;
        if (definition.Count > 3)
            displayedName = definition[3];

        try
        {
            flow = new Flow<T>()
            {
                Source = currentParent.FindClosestMatchingLeaf(entityNameA),
                Target = currentParent.FindClosestMatchingLeaf(entityNameB),
                DisplayedText = displayedName
            };
        }
        catch (AmbiguousEntityMatchException<T> e)
        {
            throw new FlowWithAmbiguousEntityException<T>(e.EntityName, e.Candidates);
        }

        if (flow.Source.Children.Count > 0)
            throw new ProcessWithChildrenConnectedException<T>(flow.Source);
        if (flow.Target.Children.Count > 0)
            throw new ProcessWithChildrenConnectedException<T>(flow.Target);

        return flow;
    }

    private ITreeNode<GraphNodeData> CreateStandardEntity(NodeType type, string name, string displayedName, ITreeNode<GraphNodeData> parent)
    {
        GraphNodeData? data = null;

        if (type == NodeType.Process)
        {
            data = new GraphNodeData() { Name = displayedName.Trim('"'), Type = NodeType.Process };
        }

        if (type == NodeType.Storage)
        {
            data = new GraphNodeData() { Name = displayedName.Trim('"'), Type = NodeType.Storage };
        }

        if (type == NodeType.InputOutput)
        {
            data = new GraphNodeData() { Name = displayedName.Trim('"'), Type = NodeType.InputOutput };
        }

        if (data == null)
            throw new InvalidEntityTypeException(type.ToString());

        return new TreeNode<GraphNodeData>() 
        { 
            EntityName = name,
            Data = data, 
            Parent = parent
        };
    }
}

