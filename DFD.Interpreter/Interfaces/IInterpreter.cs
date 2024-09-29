using DFD.DataStructures.Interfaces;

namespace DFD.Parsing.Interfaces;

public interface IInterpreter
{
    IGraph<INodeData> ToDiagram(string dfdString);
}