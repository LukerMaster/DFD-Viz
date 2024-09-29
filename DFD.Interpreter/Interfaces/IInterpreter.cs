using DFD.DataStructures.Interfaces;

namespace DFD.Parsing.Interfaces;

public interface IInterpreter<T> where T : INodeData
{
    IGraph<T> ToDiagram(string dfdString);
}