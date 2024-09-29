using DFD.DataStructures.Interfaces;

namespace DFD.Parsing.Interfaces;

public interface INodeDataFactory
{
    INodeData CreateData(string displayedName, NodeType type);
}