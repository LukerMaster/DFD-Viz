using System.Text.RegularExpressions;
using DFD.DataStructures.Implementations;
using DFD.DataStructures.Interfaces;
using DFD.Parsing.Interfaces;

namespace DFD.Parsing;

internal class GraphObjectParser
{
    public GraphObjectParser(INodeDataFactory dataFactory)
    {
        DataFactory = dataFactory;
    }

    protected INodeDataFactory DataFactory { get; }

    private readonly Dictionary<string, NodeType> _validDefinitions = new()
    {
        { "Process", NodeType.Process },
        { "Storage", NodeType.Storage },
        { "IO", NodeType.InputOutput },
    };

    public INodeRef<INodeData> TryParseNode(string line, INode<INodeData> currentParent)
    {
        INodeRef<INodeData>? node = null;

        // Split by any amount of whitespace
        var definition = SplitByWhitespace(line);

        var typeName = definition[0];
        var nodeName = definition[1];
        var displayedName = definition[1];
        if (definition.Count == 3)
        {
            displayedName = definition[2];
        }

        if (_validDefinitions.TryGetValue(typeName, out var type))
        {
            try
            {
                currentParent.AddChild(DataFactory.CreateData(displayedName.Trim('"'), type), nodeName);
            }
            catch (SameFullNodeNameException e)
            {
                throw new RedefinitionOfNodeException(nodeName);
            }
            
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

    public IFlow<INodeData> TryParseFlow(string statement, INodeRef<INodeData> currentParent)
    {
        IFlow<INodeData>? flow = null;

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
                throw new ProcessWithChildrenConnectedException<INodeData>(source);
            if (source.Children.Count > 0)
                throw new ProcessWithChildrenConnectedException<INodeData>(target);

            flow = new Flow<INodeData>()
            {
                Source = source,
                Target = target,
                Name = flowName,
                IsBidirectional = flowType == "<->"
            };
        }
        catch (AmbiguousNodeMatchException<INodeData> e)
        {
            throw new FlowWithAmbiguousNodeException<INodeData>(e.NodeName, e.Candidates);
        }
        catch (NodeNotFoundException e)
        {
            throw new UndefinedNodeReferencedException(e.NodeName, e.ParentNodeName);
        }

        return flow;
    }
}

