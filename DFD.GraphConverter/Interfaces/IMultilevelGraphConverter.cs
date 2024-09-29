using DFD.DataStructures.Interfaces;

namespace DFD.GraphConverter.Interfaces;

public interface IMultilevelGraphConverter
{
    IGraph<ICollapsibleNodeData> ToMultilevelGraph(IGraph<INodeData> graph);
}