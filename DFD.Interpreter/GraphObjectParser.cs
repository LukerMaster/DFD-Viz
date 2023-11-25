using System.Text.RegularExpressions;
using DataStructure.NamedTree;
using DFD.Model.Interfaces;
using DFD.ModelImplementations;

namespace DFD.Parsing;

internal class GraphObjectParser
{
    private readonly Dictionary<string, NodeType> _validDefinitions = new()
    {
        { "Process", NodeType.Process },
        { "Storage", NodeType.Storage },
        { "IO", NodeType.InputOutput },
    };

    public ITreeNode<IGraphNodeData> TryParseNode(string line, IModifiableTreeNode<IGraphNodeData> currentParent)
    {
        ITreeNode<IGraphNodeData>? node = null;

        // Split by any amount of whitespace
        var definition = SplitByWhitespace(line);

        var typeName = definition[0];
        var nodeName = definition[1];
        var displayedName = definition[2];

        if (_validDefinitions.TryGetValue(typeName, out var type))
        {
            node = CreateStandardNode(type, nodeName, displayedName, currentParent);
            currentParent.Children.Add(node);
        }
        else
        {
            throw new InvalidNodeTypeException(typeName);
        }
        
        return node;
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

    public INodeFlow TryParseFlow<T>(string statement, ITreeNode<T> currentParent)
    {
        INodeFlow? flow = null;

        var definition = SplitByWhitespace(statement);

        var nodeNameA = definition[0];
        var flowType = definition[1]; // TODO: do something with this value.
        var nodeNameB = definition[2];

        var flowName = String.Empty;
        if (definition.Count > 3)
            flowName = definition[3].Trim('"');

        try
        {
            var source = currentParent.FindMatchingNode(nodeNameA, leavesOnly: true);
            var target = currentParent.FindMatchingNode(nodeNameB, leavesOnly: true);

            if (source.Children.Count > 0)
                throw new ProcessWithChildrenConnectedException<T>(source);
            if (source.Children.Count > 0)
                throw new ProcessWithChildrenConnectedException<T>(target);

            flow = new NodeFlow()
            {
                SourceNodeName = source.FullNodeName,
                TargetNodeName = target.FullNodeName,
                FlowName = flowName,
                BiDirectional = flowType == "<->"
            };
        }
        catch (AmbiguousNodeMatchException<T> e)
        {
            throw new FlowWithAmbiguousNodeException<T>(e.NodeName, e.Candidates);
        }
        catch (NodeNotFoundException e)
        {
            throw new UndefinedNodeReferencedException(e.NodeName, e.ParentNodeName);
        }

        

        return flow;
    }

    private ITreeNode<IGraphNodeData> CreateStandardNode(NodeType type, string name, string displayedName, ITreeNode<IGraphNodeData> parent)
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
            throw new InvalidNodeTypeException(type.ToString());

        return new TreeNode<IGraphNodeData>() 
        { 
            NodeName = name,
            Data = data, 
            Parent = parent
        };
    }
}

