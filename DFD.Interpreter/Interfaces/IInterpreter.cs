
namespace DFD.Parsing.Interfaces;

public interface IInterpreter
{
    IGraph<IGraphNodeData> ToDiagram(string dfdString);
}