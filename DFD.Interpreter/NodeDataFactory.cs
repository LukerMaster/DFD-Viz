using DFD.DataStructures.Implementations;
using DFD.DataStructures.Interfaces;
using DFD.Parsing.Interfaces;

namespace DFD.Parsing;

public class NodeDataFactory : INodeDataFactory
{
    public INodeData CreateData(string displayedName, NodeType type)
    {
        return new NodeData()
        {
            DisplayedName = displayedName,
            Type = type
        };
    }
}