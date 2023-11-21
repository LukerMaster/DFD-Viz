using System.Diagnostics;
using System.Text.RegularExpressions;
using DataStructure.NamedTree;
using DFD.Model;
using DFD.Model.Interfaces;
using DFD.ModelImplementations;

namespace DFD.Interpreter;

internal class GraphObjectParser
{
    private readonly Dictionary<string, NodeType> _validDefinitions = new()
    {
        { "Process", NodeType.Process },
        { "Storage", NodeType.Storage },
        { "IO", NodeType.InputOutput },
    };

    public ITreeNode<IGraphNodeData> TryParseEntity(string line, IModifiableTreeNode<IGraphNodeData> currentParent)
    {
        ITreeNode<IGraphNodeData>? entity = null;

        // Split by any amount of whitespace
        var definition = SplitByWhitespace(line);

        var typeName = definition[0];
        var entityName = definition[1];
        var displayedName = definition[2];

        if (_validDefinitions.TryGetValue(typeName, out var type))
        {
            entity = CreateStandardEntity(type, entityName, displayedName, currentParent);
            currentParent.Children.Add(entity);
        }
        else
        {
            throw new InvalidEntityTypeException(typeName);
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

    public INodeFlow<T> TryParseFlow<T>(string statement, ITreeNode<T> currentParent)
    {
        INodeFlow<T>? flow = null;

        var definition = SplitByWhitespace(statement);

        var entityNameA = definition[0];
        var flowType = definition[1]; // TODO: do something with this value.
        var entityNameB = definition[2];

        var flowName = String.Empty;
        if (definition.Count > 3)
            flowName = definition[3].Trim('"');

        try
        {
            flow = new NodeFlow<T>()
            {
                Source = currentParent.FindMatchingNode(entityNameA, leavesOnly:true),
                Target = currentParent.FindMatchingNode(entityNameB, leavesOnly:true),
                FlowName = flowName,
                BiDirectional = flowType == "<->"
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

    private ITreeNode<IGraphNodeData> CreateStandardEntity(NodeType type, string name, string displayedName, ITreeNode<IGraphNodeData> parent)
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

        return new TreeNode<IGraphNodeData>() 
        { 
            NodeName = name,
            Data = data, 
            Parent = parent
        };
    }
}

